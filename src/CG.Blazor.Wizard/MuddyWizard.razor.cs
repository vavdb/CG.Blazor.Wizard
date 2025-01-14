﻿using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CG.Blazor.Wizard
{
    /// <summary>
    /// This class is the code-behind for the <see cref="MuddyWizard"/> razor 
    /// component.
    /// </summary>
    public partial class MuddyWizard : MudComponentBase, IAsyncDisposable
    {
        // *******************************************************************
        // Fields.
        // *******************************************************************

        #region Fields

        /// <summary>
        /// This field contains the inner list of wizard panels.
        /// </summary>
        private readonly IList<MuddyWizardPanel> _panels;

        /// <summary>
        /// This field indicate whether the component has been disposed.
        /// </summary>
        private bool _disposed;

        #endregion

        // *******************************************************************
        // Events.
        // *******************************************************************

        #region Events

        /// <summary>
        /// This event is raised whenever the index changes.
        /// </summary>
        [Parameter]
        public EventCallback<IndexChangedEventArgs> IndexChanged { get; set; }

        /// <summary>
        /// This event is raised whenever the wizard is finished.
        /// </summary>
        [Parameter]
        public EventCallback WizardFinished { get; set; }

        /// <summary>
        /// This event is raised whenever the wizard is cancelled.
        /// </summary>
        [Parameter]
        public EventCallback WizardCancelled { get; set; }

        
        /// <summary>
        /// Sets the style on the panel content
        /// </summary>
        [Parameter]
        public string PanelStyle { get; set; }
        
        #endregion

        // *******************************************************************
        // Properties.
        // *******************************************************************

        #region Properties

        /// <summary>
        /// This property contains the variant for the wizard buttons.
        /// </summary>
        [Parameter]
        public Variant ButtonVariant { get; set; }

        /// <summary>
        /// This property contains the color for the cancel button.
        /// </summary>
        [Parameter]
        public Color CancelColor { get; set; }

        /// <summary>
        /// This property contains the color for the cancel button.
        /// </summary>
        [Parameter]
        public string CancelCaption { get; set; } = "Caption";

        /// <summary>
        /// This property contains the color for the cancel button.
        /// </summary>
        [Parameter]
        public string CancelTooltip { get; set; } = "Cancel the wizard.";
        
        

        /// <summary>
        /// This property contains the child content for the component.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// This property contains the CSS class for the component.
        /// </summary>
        protected string Classname => new CssBuilder("mud-wizard")
            .AddClass(Class)
            .Build();

        /// <summary>
        /// This property contains the color for the wizard descriptions.
        /// </summary>
        [Parameter]
        public Color DescriptionColor { get; set; }

        /// <summary>
        /// This property contains the typography for the wizard descriptions.
        /// </summary>
        [Parameter]
        public Typo DescriptionTypo { get; set; }

        /// <summary>
        /// This property indicates whether the next button should be disabled, or not. 
        /// </summary>
        [Parameter]
        public bool DisableNext { get; set; }

        /// <summary>
        /// This property indicates whether the previous button should be disabled, or not. 
        /// </summary>
        [Parameter]
        public bool DisablePrevious { get; set; }

        /// <summary>
        /// This property contains the elevation for the component.
        /// </summary>
        [Parameter]
        public int Elevation { set; get; }

        /// <summary>
        /// This property contains the color for the finish button.
        /// </summary>
        [Parameter]
        public Color FinishColor { get; set; }

        /// <summary>
        /// This property contains the color for the finish button.
        /// </summary>
        [Parameter]
        public string FinishCaption { get; set; } = "Finish";

        /// <summary>
        /// This property contains the color for the finish button.
        /// </summary>
        [Parameter]
        public string FinishTooltip { get; set; } = "Finish the wizard.";
        
        /// <summary>
        /// This property contains the color for non-selected chips in the 
        /// wizard header.
        /// </summary>
        [Parameter]
        public Color HeaderChipColor { get; set; }

        /// <summary>
        /// This property contains the color for selected chip in the 
        /// wizard header.
        /// </summary>
        [Parameter]
        public Color HeaderChipSelectedColor { get; set; }

        /// <summary>
        /// This property contains the variant for the chips in the wizard
        /// header.
        /// </summary>
        [Parameter]
        public Variant HeaderChipVariant { get; set; }

        /// <summary>
        /// This property contains the title for the wizard.
        /// </summary>
        [Parameter]
        public string Title { get; set; }

        /// <summary>
        /// This property contains the description for the wizard.
        /// </summary>
        [Parameter]
        public string Description { get; set; }

        /// <summary>
        /// This property contains the color for the next button.
        /// </summary>
        [Parameter]
        public Color NextColor { get; set; }

        /// <summary>
        /// This property contains the caption for the next button.
        /// </summary>
        [Parameter]
        public string NextCaption { get; set; } = "Next";

        /// <summary>
        /// This property contains the tooltip for the next button.
        /// </summary>
        [Parameter]
        public string NextTooltip { get; set; } = "Move to the next step.";
        
        /// <summary>
        /// This property indicates whether the wizard should be outlined, 
        /// or not. True to outline; False otherwise.
        /// </summary>
        [Parameter]
        public bool Outlined { get; set; }

        /// <summary>
        /// This property contains the color for the previous button.
        /// </summary>
        [Parameter]
        public Color PreviousColor { get; set; }

        /// <summary>
        /// This property contains the caption for the previous button.
        /// </summary>
        [Parameter]
        public string PreviousCaption { get; set; } = "Previous";        
        
        /// <summary>
        /// This property contains the tooltip for the previous button.
        /// </summary>
        [Parameter]
        public string PreviousTooltip { get; set; } = "Move to the previous step.";                

        /// <summary>
        /// This function returns a list of the current wizard panels.
        /// </summary>
        public IList<MuddyWizardPanel> Panels() => _panels.ToList();

        /// <summary>
        /// This property contains the index of the currently selected panel.
        /// </summary>
        [Parameter]
        public int? SelectedIndex { get; set; }

        /// <summary>
        /// This property contains the currently selected panel.
        /// </summary>
        [Parameter]
        public MuddyWizardPanel SelectedPanel { get; set; }

        /// <summary>
        /// This property indicates whether to show the cancel button, or not.
        /// </summary>
        [Parameter]
        public bool ShowCancel { get; set; }

        /// <summary>
        /// This property indicates whether the chips should respond to user clicks,
        /// or not.
        /// </summary>
        [Parameter]
        public bool ActiveChips { get; set; }

        /// <summary>
        /// This property indicates whether to show the header chips, or not.
        /// </summary>
        [Parameter]
        public bool ShowChips { get; set; }

        /// <summary>
        /// This property indicates whether to show the finish button, or not.
        /// </summary>
        [Parameter]
        public bool ShowFinish { get; set; }

        /// <summary>
        /// This property indicates whether the border radius should be set
        /// to zero, or not. True to set the border radius to zero; False 
        /// otherwise.
        /// </summary>
        [Parameter]
        public bool Square { get; set; }

        /// <summary>
        /// This property contains the color for the wizard title.
        /// </summary>
        [Parameter]
        public Color TitleColor { get; set; }

        /// <summary>
        /// This property contains the typography for the wizard title.
        /// </summary>
        [Parameter]
        public Typo TitleTypo { get; set; }

        /// <summary>
        /// This property indicates whether the previous button should be
        /// disabled, or not. True if it should be disabled; False otherwise.
        /// </summary>
        protected bool IsPreviousDisabled => DisablePrevious ||
                                             !SelectedIndex.HasValue || SelectedIndex <= 0;

        /// <summary>
        /// This property indicates whether the next button should be
        /// disabled, or not. True if it should be disabled; False otherwise.
        /// </summary>
        protected bool IsNextDisabled => DisableNext ||
                                         (!SelectedIndex.HasValue || SelectedIndex >= _panels.Count - 1);

        /// <summary>
        /// This property indicates whether the finish button should be
        /// hidden, or not. True if it should be hidden; False otherwise.
        /// </summary>
        protected bool IsFinishVisible =>
            ShowFinish && (!SelectedIndex.HasValue || SelectedIndex >= _panels.Count - 1);

        /// <summary>
        /// This property indicates whether the wizard should draw a bottom border
        /// for the wizard header.
        /// </summary>
        protected bool ShowHeaderBorder => !string.IsNullOrEmpty(Title) ||
                                           !string.IsNullOrEmpty(Description) ||
                                           !string.IsNullOrEmpty(SelectedPanel?.Title) ||
                                           !string.IsNullOrEmpty(SelectedPanel?.Description);


        
        
        #endregion

        // *******************************************************************
        // Constructors.
        // *******************************************************************

        #region Constructors

        /// <summary>
        /// This constructor creates a new instance of the <see cref="MuddyWizard"/>
        /// class.
        /// </summary>
        public MuddyWizard()
        {
            // Create default values.
            _panels = new List<MuddyWizardPanel>();
            ButtonVariant = Variant.Filled;
            CancelColor = Color.Default;
            DescriptionColor = Color.Default;
            DescriptionTypo = Typo.caption;
            DisableNext = false;
            DisablePrevious = false;
            Elevation = 1;
            FinishColor = Color.Default;
            HeaderChipColor = Color.Default;
            HeaderChipSelectedColor = Color.Primary;
            HeaderChipVariant = Variant.Filled;
            NextColor = Color.Default;
            PreviousColor = Color.Default;
            ShowCancel = true;
            ShowChips = false;
            ActiveChips = false;
            ShowFinish = true;
            TitleColor = Color.Default;
            TitleTypo = Typo.h4;
            Title = "";
            Description = "";
        }

        #endregion

        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <inheritdoc />
        public void ChipSelect(
            MuddyWizardPanel panel
        )
        {
            // Do we have active chips?
            if (ActiveChips)
            {
                // Select the panel.
                Select(panel);
            }
        }

        // *******************************************************************

        /// <inheritdoc />
        public void Select(
            MuddyWizardPanel panel
        )
        {
            // Get the index for the panel.
            var index = _panels.IndexOf(panel);

            // Did we find the panel?
            if (-1 != index)
            {
                // Make the selection.
                Select(index);
            }
        }

        // *******************************************************************

        /// <summary>
        /// This method causes the wizard to navigate to the specified panel index.
        /// </summary>
        /// <param name="index">The index to use for the operation.</param>
        public void Select(
            int? index
        )
        {
            // Sanity check the index first.
            if (index == SelectedIndex)
            {
                return; // Nothing to do.
            }

            // Create arguments for the event.
            var eventArgs = new IndexChangedEventArgs()
            {
                NewIndex = index,
                NewPanel = _panels[index.Value],
                CurrentIndex = SelectedIndex,
                CurrentPanel = SelectedPanel,
            };

            // Fire the event.
            IndexChanged.InvokeAsync(eventArgs);

            // Should we cancel the navigation?
            if (eventArgs.NewIndex == eventArgs.CurrentIndex)
            {
                return; // No change to the wizard.
            }

            // Is the index within a valid range?
            if (eventArgs.NewIndex >= 0 &&
                eventArgs.NewIndex < _panels.Count)
            {
                // Update the index.
                SelectedIndex = eventArgs.NewIndex;

                // Select the current panel.
                SelectedPanel = eventArgs.NewIndex.HasValue ? _panels[eventArgs.NewIndex.Value] : null;
            }

            // Do we have a selected panel?
            if (null != SelectedPanel)
            {
                // Should we carry the wizard title to this page?
                if (string.IsNullOrEmpty(SelectedPanel.Title))
                {
#pragma warning disable BL0005
                    // Ignore warning, we want to set the title.
                    SelectedPanel.Title = Title;
#pragma warning restore BL0005
                }

                // Should we carry the wizard description to this page?
                if (string.IsNullOrEmpty(SelectedPanel.Description))
                {
#pragma warning disable BL0005
                    // Ignore warning, we want to set the description.
                    SelectedPanel.Description = Description;
#pragma warning restore BL0005
                }
            }

            // Ensure the UI is updated.
            StateHasChanged();
        }

        #endregion

        // *******************************************************************
        // Protected methods.
        // *******************************************************************

        #region Protected methods

        /// <summary>
        /// This method adds a new wizard panel to the component.
        /// </summary>
        /// <param name="panel">The wizard panel to add.</param>
        protected internal void AddPanel(
            MuddyWizardPanel panel
        )
        {
            // Add the panel to the collection.
            _panels.Add(panel);
        }

        // *******************************************************************

        /// <summary>
        /// This method removes a panel from the component.
        /// </summary>
        /// <param name="panel">The wizard panel to remove.</param>
        protected internal Task RemovePanel(
            MuddyWizardPanel panel
        )
        {
            // Remove the panel from the collection.
            _panels.Remove(panel);

            // Return the task.
            return Task.CompletedTask;
        }

        // *******************************************************************

        /// <summary>
        /// This method is called to select the next panel in the wizard.
        /// </summary>
        protected void OnNext()
        {
            // Select the next panel.
            Select(SelectedIndex + 1);
        }

        // *******************************************************************

        /// <summary>
        /// This method is called to select the previous panel in the wizard.
        /// </summary>
        protected void OnPrevious()
        {
            // Select the previous panel.
            Select(SelectedIndex - 1);
        }

        // *******************************************************************

        /// <summary>
        /// This method cancels the wizard.
        /// </summary>
        /// <returns>A task to perform the operation.</returns>
        protected async Task OnCancel()
        {
            // Raise the event.
            await WizardCancelled.InvokeAsync();
        }

        // *******************************************************************

        /// <summary>
        /// This method finishes the wizard.
        /// </summary>
        /// <returns>A task to perform the operation.</returns>
        protected async Task OnFinish()
        {
            // Raise the event.
            await WizardFinished.InvokeAsync();
        }

        // *******************************************************************

        /// <summary>
        /// This method is called when the wizard header is clicked.
        /// </summary>
        protected void OnHeaderSelect(MudChip chip)
        {
            // Do we have a selection?
            if (null != chip)
            {
                // Look for the corresponding panel.
                var panel = _panels.FirstOrDefault(
                    x =>
                        x.Title == chip.Text
                );

                // Did we find one?
                if (null != panel)
                {
                    // Update the selection.
                    Select(panel);
                }
            }
        }

        // *******************************************************************

        /// <summary>
        /// This method is called after a render operation.
        /// </summary>
        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                // Try to select the first page.
                Select(0);
            }
        }

        // *******************************************************************

        /// <summary>
        /// This method is called to dispose of the component.
        /// </summary>
        /// <returns>A <see cref="ValueTask"/> for the operation.</returns>
        public ValueTask DisposeAsync()
        {
            // Have we already been disposed?
            if (_disposed)
            {
                return ValueTask.CompletedTask;
            }

            // Mark that we've been disposed.
            _disposed = true;

            // Prevent derived types from having to implement IDisposable.
            GC.SuppressFinalize(this);

            return ValueTask.CompletedTask;
        }

        #endregion
    }
}