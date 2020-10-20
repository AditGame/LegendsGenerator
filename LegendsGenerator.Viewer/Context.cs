// -------------------------------------------------------------------------------------------------
// <copyright file="Context.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Viewer
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Media;
    using System.Windows.Threading;
    using LegendsGenerator.Compiler.CSharp;
    using LegendsGenerator.Contracts;
    using LegendsGenerator.Contracts.Compiler;
    using LegendsGenerator.Contracts.Things;
    using LegendsGenerator.Viewer.Views;

    /// <summary>
    /// The context of all the data.
    /// </summary>
    public class Context : INotifyPropertyChanged
    {
        /// <summary>
        /// The last created Context instance.
        /// </summary>
        private static Context instance = new Context();

        /// <summary>
        /// The backing field for current step.
        /// </summary>
        private int currentStep;

        /// <summary>
        /// The backing field for selected square.
        /// </summary>
        private GridSquare? selectedSquare;

        /// <summary>
        /// The backing field for selected thing.
        /// </summary>
        private ThingView? selectedThing;

        /// <summary>
        /// The backing field for all the squares.
        /// </summary>
        private List<SquareView> squares = new List<SquareView>();

        /// <summary>
        /// The backing field for FollowThing.
        /// </summary>
        private bool followThing;

        /// <summary>
        /// The backing field for DebugAtThingId.
        /// </summary>
        private Guid? debugAtThingId;

        /// <summary>
        /// Backing field for IsGeneratingHistory.
        /// </summary>
        private bool isGeneratingHistory;

        /// <summary>
        /// Backing field for IsHistoryGenerationCancellationRequested.
        /// </summary>
        private bool isHistoryGenerationCancellationRequested;

        /// <summary>
        /// Initializes a new instance of the <see cref="Context"/> class.
        /// </summary>
        private Context()
        {
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets the last created instance of this class.
        /// </summary>
        public static Context Instance => instance;

        /// <summary>
        /// Gets the steps of the world.
        /// </summary>
        public IDictionary<int, World> WorldSteps { get; } = new Dictionary<int, World>();

        /// <summary>
        /// Gets the history.
        /// </summary>
        public HistoryGenerator? History { get; private set; }

        /// <summary>
        /// Gets or sets the dispatcher to use for non-backend thread stuff.
        /// </summary>
        public Dispatcher? Dispatcher { get; set; }

        /// <summary>
        /// Gets the condition compiler.
        /// </summary>
        public IConditionCompiler<BaseGlobalVariables>? Compiler { get; private set; }

        /// <summary>
        /// Gets or sets the selected square on the map.
        /// </summary>
        public GridSquare? SelectedSquare
        {
            get => this.selectedSquare;

            set
            {
                this.selectedSquare = value;
                this.ThingsInSquare.Clear();
                if (this.selectedSquare != null)
                {
                    // Do not include the square definition in the list of things to show, as that has a special spot at the top.
                    this.selectedSquare.GetThings(true).ToList().ForEach(t => this.ThingsInSquare.Add(new ThingView(t, this.CurrentWorld)));

                    this.SelectedThing = this.ThingsInSquare.FirstOrDefault();
                }

                this.OnPropertyChanged(nameof(this.SelectedSquare));
                this.OnPropertyChanged(nameof(this.PathSquares));
                this.OnPropertyChanged(nameof(this.SelectedWorldSquare));
            }
        }

        /// <summary>
        /// Gets the selected world square.
        /// </summary>
        public ThingView? SelectedWorldSquare
        {
            get
            {
                var def = this.SelectedSquare?.SquareDefinition;
                if (def != null)
                {
                    return new ThingView(def, this.CurrentWorld);
                }

                return null;
            }
        }

        /// <summary>
        /// Gets or sets the selected thing.
        /// </summary>
        public ThingView? SelectedThing
        {
            get => this.selectedThing;

            set
            {
                this.selectedThing = value;

                this.OnPropertyChanged(nameof(this.SelectedThing));
                this.OnPropertyChanged(nameof(this.Lines));
                this.OnPropertyChanged(nameof(this.PathSquares));
                this.OnPropertyChanged(nameof(this.DebugAtSelectedThing));
            }
        }

        /// <summary>
        /// Gets or sets the current step.
        /// </summary>
        public int CurrentStep
        {
            get
            {
                return this.currentStep;
            }

            set
            {
                if (value < 0)
                {
                    value = 0;
                }

                if (this.IsGeneratingHistory)
                {
                    // Fast exit if world generation is ongoing.
                    return;
                }

                if (!this.WorldSteps.ContainsKey(value))
                {
                    this.IsGeneratingHistory = true;
                    this.BackfillToStep(value);
                    this.IsGeneratingHistory = false;
                }

                if (value <= this.MaxSteps)
                {
                    this.currentStep = value;
                }
                else
                {
                    this.currentStep = this.MaxSteps;
                }

                // Reset cancellation if it's been set.
                this.HistoryGenerationCancellationRequested = false;

                Action action = () =>
                {
                    this.OccurredEvents.Clear();
                    this.CurrentWorld.OccurredEvents.ToList().ForEach(this.OccurredEvents.Add);

                    Guid? selectedThingId = this.SelectedThing?.ThingId;

                    this.Squares = this.CurrentWorld.Grid.GetAllGridEntries().Select(x => new SquareView(x.Square)).ToList();

                    this.OnPropertyChanged(nameof(this.CurrentStep));
                    this.OnPropertyChanged(nameof(this.CurrentWorld));

                    // This has to occur after the above notifications, as those change around the squares.
                    if (this.FollowThing && selectedThingId.HasValue && this.CurrentWorld.TryFindThing(selectedThingId.Value, out BaseThing? result))
                    {
                        // If FollowThing is set, follow the thing as it moves.
                        this.SelectedSquare = this.CurrentWorld.Grid.GetSquare(result.X, result.Y);
                        this.SelectedThing = new ThingView(result, this.CurrentWorld);
                    }
                    else if (this.SelectedSquare != null)
                    {
                        // Keep the current grid in focus otherwise.
                        this.SelectedSquare = this.CurrentWorld.Grid.GetSquare(this.SelectedSquare.X, this.SelectedSquare.Y);
                    }
                };

                if (this.Dispatcher != null)
                {
                    this.Dispatcher.BeginInvoke(action);
                }
                else
                {
                    action();
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether history generation is NOT in process.
        /// </summary>
        public bool IsNotGeneratingHistory => !this.IsGeneratingHistory;

        /// <summary>
        /// Gets a value indicating whether history generation is in process.
        /// </summary>
        public bool IsGeneratingHistory
        {
            get
            {
                return this.isGeneratingHistory;
            }

            private set
            {
                this.isGeneratingHistory = value;
                this.OnPropertyChanged(nameof(this.IsGeneratingHistory));
                this.OnPropertyChanged(nameof(this.IsNotGeneratingHistory));
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether history generation has been requested to cancel.
        /// </summary>
        public bool HistoryGenerationCancellationRequested
        {
            get
            {
                return this.isHistoryGenerationCancellationRequested;
            }

            set
            {
                if (!this.isGeneratingHistory)
                {
                    this.isHistoryGenerationCancellationRequested = false;
                }
                else
                {
                    this.isHistoryGenerationCancellationRequested = value;
                }

                this.OnPropertyChanged(nameof(this.HistoryGenerationCancellationRequested));
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the currently selected thing will retain focus as history progresses.
        /// </summary>
        public bool FollowThing
        {
            get
            {
                return this.followThing;
            }

            set
            {
                this.followThing = value;
                this.OnPropertyChanged(nameof(this.FollowThing));
            }
        }

        /// <summary>
        /// Gets the current world.
        /// </summary>
        public World CurrentWorld
        {
            get
            {
                return this.WorldSteps[this.CurrentStep];
            }
        }

        /// <summary>
        /// Gets the number of loaded steps.
        /// </summary>
        public int MaxSteps
        {
            get
            {
                return this.WorldSteps.Count - 1;
            }
        }

        /// <summary>
        /// Gets the thing ID to pause debugging at.
        /// </summary>
        public Guid? DebugAtThingId
        {
            get
            {
                return this.debugAtThingId;
            }

            private set
            {
                this.debugAtThingId = value;
                this.OnPropertyChanged(nameof(this.DebugAtSelectedThing));
                this.OnPropertyChanged(nameof(this.DebugAtThingId));
            }
        }

        /// <summary>
        /// Gets a value indicating whether debugging is available (Only available for debug builds).
        /// </summary>
#pragma warning disable CA1822 // Mark members as static. Oddity to conform to binding.
        public bool CanDebugAtThing
#pragma warning restore CA1822 // Mark members as static
        {
            get
            {
#if DEBUG
                return true;
#else
                return false;
#endif
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the currently selected thing should cause the debugger to start.
        /// </summary>
        public bool DebugAtSelectedThing
        {
            get
            {
                if (this.SelectedThing == null)
                {
                    return false;
                }

                return this.DebugAtThingId == this.SelectedThing.ThingId;
            }

            set
            {
                if (this.SelectedThing != null && value == true)
                {
                    this.DebugAtThingId = this.SelectedThing.ThingId;
                }
                else
                {
                    this.DebugAtThingId = null;
                }
            }
        }

        /// <summary>
        /// Gets the list of occurred events for the previous step.
        /// </summary>
        public ObservableCollection<OccurredEvent> OccurredEvents { get; } = new ObservableCollection<OccurredEvent>();

        /// <summary>
        /// Gets the list of things in an event.
        /// </summary>
        public ObservableCollection<ThingView> ThingsInSquare { get; } = new ObservableCollection<ThingView>();

        /// <summary>
        /// Gets or sets all squares currently visible.
        /// </summary>
        public List<SquareView> Squares
        {
            get
            {
                return this.squares;
            }

            set
            {
                this.squares = value;
                this.OnPropertyChanged(nameof(this.Squares));
            }
        }

        /// <summary>
        /// Gets all lines currently visible.
        /// </summary>
        public List<LineView> Lines
        {
            get
            {
                List<LineView> lines = new List<LineView>();

                if (this.SelectedThing != null)
                {
                    lines.AddRange(this.SelectedThing.ReleventLines);
                }

                return lines;
            }
        }

        /// <summary>
        /// Gets all lines currently visible.
        /// </summary>
        public List<PathSquareView> PathSquares
        {
            get
            {
                List<PathSquareView> paths = new List<PathSquareView>();

                if (this.SelectedThing != null)
                {
                    paths.AddRange(this.SelectedThing.ReleventPathParts);
                }

                if (this.SelectedSquare != null)
                {
                    paths.Add(new PathSquareView(this.SelectedSquare.X, this.SelectedSquare.Y, 1, Brushes.Red, 0.5f));
                }

                return paths;
            }
        }

        /// <summary>
        /// Gets the width of the viewer.
        /// </summary>
        public int ViewerWidth => this.CurrentWorld.Grid.Width * WorldViewer.GridSize;

        /// <summary>
        /// Gets the height of the viewer.
        /// </summary>
        public int ViewerHeight => this.CurrentWorld.Grid.Height * WorldViewer.GridSize;

        /// <summary>
        /// Attaches information to this context.
        /// </summary>
        /// <param name="history">The history.</param>
        /// <param name="initialWorld">The initial world state.</param>
        /// <param name="compiler">The compiler.</param>
        public void Attach(HistoryGenerator history, World initialWorld, IConditionCompiler<BaseGlobalVariables> compiler)
        {
            this.History = history;
            this.Compiler = compiler;
            this.WorldSteps[0] = initialWorld;
            this.CurrentStep = 0;
        }

        /// <summary>
        /// Advances the world state to the next step.
        /// </summary>
        public void NextStep()
        {
            this.CurrentStep++;
        }

        /// <summary>
        /// Regresses the world state to the previous step.
        /// </summary>
        public void PreviousStep()
        {
            if (this.CurrentStep != 0)
            {
                this.CurrentStep--;
            }
        }

        /// <summary>
        /// Moves the selected thing to the next thing in the square.
        /// </summary>
        public void NextThingInSquare()
        {
            if (this.SelectedSquare == null || this.SelectedThing == null)
            {
                // No op if nothing is selected.
                return;
            }

            var things = this.ThingsInSquare.ToList();
            int indexOfCurrentThing = things.FindIndex(t => t.Equals(this.SelectedThing));
            var newIndex = (indexOfCurrentThing != things.Count - 1) ? indexOfCurrentThing + 1 : 0;
            this.SelectedThing = things[newIndex];
        }

        /// <summary>
        /// Moves the selected thing to the previous thing in the square.
        /// </summary>
        public void PreviousThingInSquare()
        {
            if (this.SelectedSquare == null || this.SelectedThing == null)
            {
                // No op if nothing is selected.
                return;
            }

            var things = this.ThingsInSquare.ToList();
            int indexOfCurrentThing = things.FindIndex(t => t.Equals(this.SelectedThing));
            var newIndex = (indexOfCurrentThing != 0) ? indexOfCurrentThing - 1 : things.Count - 1;
            this.SelectedThing = things[newIndex];
        }

        /// <summary>
        /// Fires property changed events.
        /// </summary>
        /// <param name="propertyName">True when property changed.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Gets a particular step, backfilling data as needed.
        /// </summary>
        /// <param name="toStep">The step to load to.</param>
        private void BackfillToStep(int toStep)
        {
            if (this.History == null || this.Compiler == null)
            {
                throw new InvalidOperationException("Must attach the world before changing history.");
            }

            if (!this.WorldSteps.ContainsKey(toStep - 1))
            {
                this.BackfillToStep(toStep - 1);
            }

            if (this.HistoryGenerationCancellationRequested)
            {
                // If generation cancellation is requested, don't generate this step.
                return;
            }

#if DEBUG
            this.History.OpenDebuggerAtThing = this.DebugAtThingId;
#endif
            this.Compiler.UpdateGlobalVariables(x => x.World = this.WorldSteps[toStep - 1]);
            this.WorldSteps[toStep] = this.History.Step(this.WorldSteps[toStep - 1]);
            this.OnPropertyChanged(nameof(this.MaxSteps));
        }
    }
}
