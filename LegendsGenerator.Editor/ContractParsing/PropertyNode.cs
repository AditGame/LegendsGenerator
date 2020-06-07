// -------------------------------------------------------------------------------------------------
// <copyright file="PropertyNode.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Editor.ContractParsing
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Media;

    using LegendsGenerator.Contracts.Definitions;
    using LegendsGenerator.Contracts.Definitions.Validation;

    /// <summary>
    /// Represents a node of the contract.
    /// </summary>
    public abstract class PropertyNode : INotifyPropertyChanged
    {
        /// <summary>
        /// The underlying string field.
        /// </summary>
        private string name;

        /// <summary>
        /// The function which changes the name.
        /// </summary>
        private Action<string>? changeName;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyNode"/> class.
        /// </summary>
        /// <param name="thing">The thing this node points to.</param>
        /// <param name="property">The property info.</param>
        /// <param name="options">The options for this node.</param>
        /// <param name="readOnly">If this instance should be read only.</param>
        public PropertyNode(
            object? thing,
            ElementInfo property,
            IEnumerable<PropertyInfo> options,
            bool readOnly = false)
        {
            this.name = property.Name;
            this.Description = property.Description;
            this.Nullable = property.Nullable;
            this.ContentsModifiable = !readOnly;

            this.changeName = property.ChangeName;

            this.GetContentsFunc = property.GetValue;
            this.NameCreatesVariableName = property.NameCreatesVariableName;
            this.ControlsDefinitionName = property.ControlsDefinitionName;
            this.ContentsType = property.PropertyType;

            if (!readOnly)
            {
                this.SetContentsFunc = property.SetValue;
            }

            foreach (PropertyInfo option in options)
            {
                PropertyNode? node = DefinitionParser.ToNode(thing, option);
                if (node != null)
                {
                    this.AddOption(node);
                }
            }

            // Set up that changes to Content or underlying nodes causes validation changes.
            this.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName?.Equals(nameof(this.Content)) ?? false)
                {
                    this.OnPropertyChanged(nameof(this.ValidationFailures));
                }
            };

            foreach (PropertyNode node in this.Nodes)
            {
                node.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName?.Equals(nameof(node.ValidationFailures)) ?? false)
                    {
                        this.OnPropertyChanged(nameof(this.ValidationFailures));
                    }
                };
            }

            foreach (PropertyNode node in this.Options)
            {
                node.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName?.Equals(nameof(node.ValidationFailures)) ?? false)
                    {
                        this.OnPropertyChanged(nameof(this.ValidationFailures));
                    }
                };
            }

            this.Nodes.CollectionChanged += this.NodeListItemsChanged;

            this.Options.CollectionChanged += this.NodeListItemsChanged;
        }

        /// <summary>
        /// Notifies when a property is changed.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets or sets the name of the node, typically either the property name or the dictionary key.
        /// </summary>
        public string Name
        {
            get => this.name;

            set
            {
                if (this.changeName == null)
                {
                    throw new InvalidOperationException("Name can not be changed.");
                }

                this.changeName(value);
            }
        }

        /// <summary>
        /// Gets a value indicating whether changing the name of this node is permitted.
        /// </summary>
        public bool NameCanBeChanged => this.changeName != null;

        /// <summary>
        /// Gets a value indicating whether changing the name of this node is not permitted.
        /// </summary>
        public bool NameCanNotBeChanged => this.changeName == null;

        /// <summary>
        /// Gets the description of this node.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets a value indicating whether this node can be set to null.
        /// </summary>
        public bool Nullable { get; }

        /// <summary>
        /// Gets a value indicating whether the name represents a variable name.
        /// </summary>
        public bool NameCreatesVariableName { get; }

        /// <summary>
        /// Gets a value indicating whether a change to the contents should cause the definition name to change.
        /// </summary>
        public bool ControlsDefinitionName { get; }

        /// <summary>
        /// Gets or sets the contents of the node; null if there's no contents beyond sub nodes.
        /// </summary>
        public object? Content
        {
            get
            {
                return this.GetContentsFunc?.Invoke();
            }

            set
            {
                if (!this.ContentsModifiable)
                {
                    throw new InvalidOperationException("Can not modify contents");
                }

                if (this.SetContentsFunc == null)
                {
                    throw new InvalidOperationException($"{nameof(this.SetContentsFunc)} msut be attached to set contents.");
                }

                this.SetContentsFunc.Invoke(value);
                this.OnPropertyChanged(nameof(this.Content));
                this.OnPropertyChanged(nameof(this.ShowControl));
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this node can be renamed.
        /// </summary>
        public bool Renamable { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether the contents are modifiable.
        /// </summary>
        public bool ContentsModifiable { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether the subnodes can be modified.
        /// </summary>
        public bool SubNodesModifiable { get; protected set; }

        /// <summary>
        /// Gets the upstream node.
        /// </summary>
        public PropertyNode? UpstreamNode { get; private set; }

        /// <summary>
        /// Gets the list of additional options which can be added to this node.
        /// </summary>
        public ObservableCollection<PropertyNode> Options { get; } = new ObservableCollection<PropertyNode>();

        /// <summary>
        /// Gets the list of subnodes on this node.
        /// </summary>
        public ObservableCollection<PropertyNode> Nodes { get; } = new ObservableCollection<PropertyNode>();

        /// <summary>
        /// Gets the list of validation failures on this node.
        /// </summary>
        public IList<ValidationIssue> ValidationFailures
        {
            get
            {
                List<ValidationIssue> failures = new List<ValidationIssue>();
                failures.AddRange(this.GetLevelIssues());
                this.Options.ToList().ForEach(node => failures.AddRange(node.ValidationFailures.Select(v => v.Clone(node.Name))));
                this.Nodes.ToList().ForEach(node => failures.AddRange(node.ValidationFailures.Select(v => v.Clone(node.Name))));
                return failures;
            }
        }

        /// <summary>
        /// Gets the text color to use for the title.
        /// </summary>
        public Brush GetTextColor
        {
            get
            {
                if (this.ValidationFailures.Any(v => v.Level == ValidationLevel.Info))
                {
                    return Brushes.Blue;
                }
                else if (this.ValidationFailures.Any(v => v.Level == ValidationLevel.Warning))
                {
                    return Brushes.Orange;
                }
                else if (this.ValidationFailures.Any(v => v.Level == ValidationLevel.Error))
                {
                    return Brushes.Red;
                }
                else
                {
                    return Brushes.Black;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether creating can be done on this node.
        /// </summary>
        public virtual bool CanCreate => this.Content == null;

        /// <summary>
        /// Gets a value indicating whether deleting can be done on this node.
        /// </summary>
        public virtual bool CanDelete => this.Content != null && this.Nullable;

        /// <summary>
        /// Gets a value indicating whether the control should be shown.
        /// </summary>
        public virtual bool ShowControl => this.Content != null;

        /// <summary>
        /// Gets or sets a function which returns the contents of this node.
        /// </summary>
        protected Func<object?>? GetContentsFunc { get; set; }

        /// <summary>
        /// Gets or sets a function which sets the contents of this node.
        /// </summary>
        protected Action<object?>? SetContentsFunc { get; set; }

        /// <summary>
        /// Gets or sets the type of the contents.
        /// </summary>
        protected Type ContentsType { get; set; }

        /// <summary>
        /// Adds a node, setting up the upstream.
        /// </summary>
        /// <param name="node">The node to add.</param>
        public void AddNode(PropertyNode node)
        {
            node.UpstreamNode = this;
            this.Nodes.Add(node);
        }

        /// <summary>
        /// Adds an option, setting up the upstream.
        /// </summary>
        /// <param name="node">The node to add.</param>
        public void AddOption(PropertyNode node)
        {
            node.UpstreamNode = this;
            this.Options.Add(node);
        }

        /// <summary>
        /// Renames this instance.
        /// </summary>
        /// <param name="name">The new name to apply.</param>
        public virtual void Rename(string name)
        {
            throw new NotImplementedException($"{nameof(this.Rename)} is not implemented.");
        }

        /// <summary>
        /// Handles create events.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The args.</param>
        public virtual void HandleCreate(object sender, RoutedEventArgs e)
        {
            object? def;
            if (this.ContentsType == typeof(string))
            {
                def = BaseDefinition.UnsetString;
            }
            else
            {
                def = Activator.CreateInstance(this.ContentsType);
            }

            this.Content = def;
            this.OnPropertyChanged(nameof(this.CanCreate));
            this.OnPropertyChanged(nameof(this.CanDelete));
        }

        /// <summary>
        /// Handles delete events.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The args.</param>
        public virtual void HandleDelete(object sender, RoutedEventArgs e)
        {
            this.Content = null;
            this.OnPropertyChanged(nameof(this.CanCreate));
            this.OnPropertyChanged(nameof(this.CanDelete));
        }

        /// <summary>
        /// Converts an action to be an object action.
        /// </summary>
        /// <typeparam name="T">The typical input type.</typeparam>
        /// <param name="action">The action to convert.</param>
        /// <returns>The action, which now takes object? as input.</returns>
        protected static Action<object?> ConvertAction<T>(Action<T> action)
        {
            return new Action<object?>(input =>
            {
                if (input is T asT)
                {
                    action(asT);
                }
                else
                {
                    throw new InvalidOperationException($"Must set as {typeof(T).Name}.");
                }
            });
        }

        /// <summary>
        /// Gets the issues present on this specific level of definition.
        /// </summary>
        /// <returns>The list of issues on this level.</returns>
        protected virtual List<ValidationIssue> GetLevelIssues()
        {
            List<ValidationIssue> issues = new List<ValidationIssue>();

            if (string.IsNullOrWhiteSpace(this.Name))
            {
                issues.Add(new ValidationIssue(ValidationLevel.Error, "Name can not be null, empty, or whitespace."));
            }
            else if (this.Name.Equals(BaseDefinition.UnsetString))
            {
                issues.Add(new ValidationIssue(
                    ValidationLevel.Error,
                    "Name can not be the default string."));
            }
            else if (this.NameCreatesVariableName)
            {
                if (!ValidVariableName(this.Name))
                {
                    issues.Add(new ValidationIssue(
                        ValidationLevel.Error,
                        "Name must only consist of letters, numbers, and understores, also can not start with a number."));
                }
                else if (!char.IsUpper(this.Name.First()))
                {
                    issues.Add(new ValidationIssue(
                        ValidationLevel.Info,
                        "Name should start with an uppercase letter."));
                }
            }

            if (!this.Nullable && this.Content == null)
            {
                issues.Add(new ValidationIssue(ValidationLevel.Error, "Contents can not be null."));
            }

            return issues;
        }

        /// <summary>
        /// Fires property changed events.
        /// </summary>
        /// <param name="propertyName">True when property changed.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            // If the vlidation changed, than the text color changed as well.
            if (propertyName.Equals(nameof(this.ValidationFailures)))
            {
                this.OnPropertyChanged(nameof(this.GetTextColor));
            }
        }

        /// <summary>
        /// Validates that the input string can be used as a string.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>True if the input is a valid name, false otherwise.</returns>
        private static bool ValidVariableName(string name)
        {
            // Alphanumeric and underscores only.
            if (name.Any(c => !char.IsLetterOrDigit(c) && c != '_'))
            {
                return false;
            }

            // Can't start with a number.
            if (char.IsNumber(name.First()))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Action preformed when a node list changes.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event.</param>
        private void NodeListItemsChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.OnPropertyChanged(nameof(this.ValidationFailures));

            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add && e.NewItems != null)
            {
                foreach (PropertyNode node in e.NewItems.OfType<PropertyNode>())
                {
                    node.PropertyChanged += (s, e) =>
                    {
                        if (e.PropertyName?.Equals(nameof(node.ValidationFailures)) ?? false)
                        {
                            this.OnPropertyChanged(nameof(this.ValidationFailures));
                        }
                    };
                }
            }
        }
    }
}
