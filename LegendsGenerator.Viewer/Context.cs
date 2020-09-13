// -------------------------------------------------------------------------------------------------
// <copyright file="Context.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Viewer
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using LegendsGenerator.Viewer.Views;

    /// <summary>
    /// The context of all the data.
    /// </summary>
    public class Context : INotifyPropertyChanged
    {
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
        /// Initializes a new instance of the <see cref="Context"/> class.
        /// </summary>
        /// <param name="history">The history machine.</param>
        /// <param name="initialWorld">The initial world state.</param>
        public Context(HistoryMachine history, World initialWorld)
        {
            this.History = history;
            this.WorldSteps[0] = initialWorld;
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets the steps of the world.
        /// </summary>
        public IDictionary<int, World> WorldSteps { get; } = new Dictionary<int, World>();

        /// <summary>
        /// Gets the history.
        /// </summary>
        public HistoryMachine History { get; }

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
                    this.selectedSquare.ThingsInSquare.ToList().ForEach(t => this.ThingsInSquare.Add(new ThingView(t)));

                    if (this.ThingsInSquare.Any())
                    {
                        this.SelectedThing = this.ThingsInSquare.First();
                    }
                }

                this.OnPropertyChanged(nameof(this.SelectedSquare));
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

                if (!this.WorldSteps.ContainsKey(value))
                {
                    this.BackfillToStep(value);
                    this.OnPropertyChanged(nameof(this.MaxSteps));
                }

                this.currentStep = value;
                this.OccurredEvents.Clear();
                this.CurrentWorld.OccurredEvents.ToList().ForEach(this.OccurredEvents.Add);

                this.OnPropertyChanged(nameof(this.CurrentStep));
                this.OnPropertyChanged(nameof(this.CurrentWorld));
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
        /// Gets the list of occurred events for the previous step.
        /// </summary>
        public ObservableCollection<OccurredEvent> OccurredEvents { get; } = new ObservableCollection<OccurredEvent>();

        /// <summary>
        /// Gets the list of things in an event.
        /// </summary>
        public ObservableCollection<ThingView> ThingsInSquare { get; } = new ObservableCollection<ThingView>();

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
            if (!this.WorldSteps.ContainsKey(toStep - 1))
            {
                this.BackfillToStep(toStep - 1);
            }

            this.WorldSteps[toStep] = this.History.Step(this.WorldSteps[toStep - 1]);
        }
    }
}
