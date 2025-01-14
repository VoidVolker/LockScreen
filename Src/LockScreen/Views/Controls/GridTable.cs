using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;

using LockScreen.DataTypes.Events;
using LockScreen.DataTypes.Properties;

namespace LockScreen.Views.Controls
{
    /// <summary>
    /// Simple table for items inherited from Grid control
    /// </summary>
    /// <remarks>
    /// Set "Orientation" and "[Rows/Columns]HeaderEnabled" properties, then use
    /// ColumnTemplates/RowTemplates with one standard RowDefinition/ColumnDefinition.
    /// It's *Templates inherited from *Definition and GridTable uses them to generate
    /// definitions for header and items in runtime.
    /// </remarks>
    public class GridTable : Grid
    {
        #region Public Constructors

        public GridTable()
        {
            ColumnTemplates = [];
            RowTemplates = [];
            Loaded += GridTable_Loaded;
        }

        private void GridTable_Loaded(object sender, RoutedEventArgs e)
        {
            BuildTable();
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Is column header enabled or not
        /// </summary>
        public bool ColumnsHeaderEnabled
        {
            get { return (bool)GetValue(ColumnsHeaderEnabledProperty); }
            set { SetValue(ColumnsHeaderEnabledProperty, value); }
        }

        /// <summary>
        /// Columns templates
        /// </summary>
        public ObservableCollection<GridTableColumn> ColumnTemplates
        {
            get { return (ObservableCollection<GridTableColumn>)GetValue(ColumnTemplatesProperty); }
            set { SetValue(ColumnTemplatesProperty, value); }
        }

        /// <summary>
        /// Items source property
        /// </summary>
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        /// <summary>
        /// Items orientation: default is horizontal
        /// </summary>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        /// <summary>
        /// Is row header enabled or not
        /// </summary>
        public bool RowsHeaderEnabled
        {
            get { return (bool)GetValue(RowsHeaderEnabledProperty); }
            set { SetValue(RowsHeaderEnabledProperty, value); }
        }

        /// <summary>
        /// Columns templates
        /// </summary>
        public ObservableCollection<GridTableRow> RowTemplates
        {
            get { return (ObservableCollection<GridTableRow>)GetValue(RowTemplatesProperty); }
            set { SetValue(RowTemplatesProperty, value); }
        }

        #endregion Public Properties

        #region Public Fields

        public static readonly DependencyProperty ColumnsHeaderEnabledProperty =
            DP<GridTable>.R(x => x.ColumnsHeaderEnabled, false);

        public static readonly DependencyProperty ColumnTemplatesProperty =
                    DP<GridTable>.R(x => x.ColumnTemplates);

        public static readonly DependencyProperty ItemsSourceProperty =
            DP<GridTable>.R(x => x.ItemsSource, x => x.OnItemsSourceChanged);

        public static readonly DependencyProperty OrientationProperty =
                    DP<GridTable>.R(x => x.Orientation);

        public static readonly DependencyProperty RowsHeaderEnabledProperty =
                            DP<GridTable>.R(x => x.RowsHeaderEnabled, false);

        public static readonly DependencyProperty RowTemplatesProperty =
            DP<GridTable>.R(x => x.RowTemplates);

        #endregion Public Fields

        #region Public Methods

        // TODO:
        // - debug multicalls
        // - item INotifyPropertyChanged support?
        // - add items cache?
        // - collection changed logic?
        public void BuildTable()
        {
            if (ItemsSource is null || ColumnTemplates is null || RowTemplates is null) { return; }

            // Check is headres enabled and templated
            bool isColumnsTemplated = ColumnsHeaderEnabled && ColumnTemplates.Count > 0;
            bool isRowsTemplated = RowsHeaderEnabled && RowTemplates.Count > 0;

            // Table full clear
            Children.Clear();

            int itemsStartColumn = 0;
            int itemsStartRow = 0;

            if (Orientation == Orientation.Horizontal)
            {
                if (isColumnsTemplated)
                {
                    const int columnHeaderRowIndex = 0;
                    int columnIndex = 0;

                    // Clear column definitions
                    ColumnDefinitions.Clear();

                    // Add headers row from column templates and column definitions
                    foreach (GridTableColumn column in ColumnTemplates)
                    {
                        // Add column definition
                        ColumnDefinitions.Add(column);

                        if (column.HeaderTemplate is not null)
                        {
                            Control headerCell = new()
                            {
                                DataContext = DataContext, // Pass current data context
                                Template = column.HeaderTemplate
                            };
                            Children.Add(headerCell);
                            SetRow(headerCell, columnHeaderRowIndex);
                            SetColumn(headerCell, columnIndex);
                        }

                        // Next column
                        columnIndex++;
                    }

                    // Set items start row
                    itemsStartRow++;

                    if (RowDefinitions.Count > 1)
                    {
                        RowDefinitions.RemoveRange(1, RowDefinitions.Count - 1); /// TODO: logic to compute diff and remove or add rows?
                    }

                    //  Add items rows
                    int itemRowIndex = itemsStartRow; // Select first row
                    foreach (object item in ItemsSource)
                    {
                        int itemColumnIndex = itemsStartColumn; // Select first column

                        // Add new row definition
                        RowDefinition rowDef = new()
                        {
                            Height = GridLength.Auto
                        };
                        RowDefinitions.Add(rowDef);

                        // And add cells for each template in templates list
                        foreach (GridTableColumn column in ColumnTemplates)
                        {
                            if (column.CellTemplate is not null)
                            {
                                Control cell = new()
                                {
                                    DataContext = item,
                                    Template = column.CellTemplate
                                };

                                Children.Add(cell);

                                SetRow(cell, itemRowIndex);
                                SetColumn(cell, itemColumnIndex);
                            }

                            // Select next column
                            itemColumnIndex++;
                        }

                        // Select next row
                        itemRowIndex++;
                    }
                }
                //// TODO: headerless items rendering
                //else {
                //}
            }
            else if (Orientation == Orientation.Vertical)
            {
                if (isRowsTemplated)
                {
                    int rowIndex = 0;
                    const int rowHeaderColumnIndex = 0;

                    // Clear rows definitions
                    RowDefinitions.Clear();

                    // Add headers column from row tempaltes
                    foreach (GridTableRow row in RowTemplates)
                    {
                        // Add row definition
                        RowDefinitions.Add(row);

                        if (row.HeaderTemplate is not null)
                        {
                            Control headerCell = new()
                            {
                                DataContext = DataContext, // Pass current data context
                                Template = row.HeaderTemplate
                            };
                            Children.Add(headerCell);
                            SetRow(headerCell, rowIndex);
                            SetColumn(headerCell, rowHeaderColumnIndex);
                        }

                        // Next row
                        rowIndex++;
                    }

                    itemsStartColumn++;

                    if (ColumnDefinitions.Count > 1)
                    {
                        ColumnDefinitions.RemoveRange(1, ColumnDefinitions.Count - 1); /// TODO: logic to compute diff and remove or add columns?
                    }

                    // Add items rows
                    int itemColumnIndex = itemsStartColumn; // Select first column
                    foreach (object item in ItemsSource)
                    {
                        int itemRowIndex = itemsStartRow; // Select first row

                        // Add new column definition
                        ColumnDefinition columnDef = new()
                        {
                            Width = GridLength.Auto
                        };
                        ColumnDefinitions.Add(columnDef);

                        // And add cells for each template in templates list
                        foreach (GridTableRow row in RowTemplates)
                        {
                            if (row.CellTemplate is not null)
                            {
                                Control cell = new()
                                {
                                    DataContext = item,
                                    Template = row.CellTemplate
                                };

                                Children.Add(cell);
                                SetRow(cell, itemRowIndex);
                                SetColumn(cell, itemColumnIndex);
                            }

                            // Select next row
                            itemRowIndex++;
                        }

                        // Select next column
                        itemColumnIndex++;
                    }
                }
                //// TODO: headerless items rendering
                //else {
                //}
            }
        }

        public void OnItemsSourceChanged(DependencyPropertyChangedEventArgs<IEnumerable> e)
        {
            // disabled: additional logic is required
            //if (e.OldValue is INotifyCollectionChanged collection1)
            //{
            //    collection1.CollectionChanged -= OnCollectionChanged;
            //}

            if (e.OldValue is INotifyPropertyChanged collection2)
            {
                collection2.PropertyChanged -= OnPropertyChanged;
            }

            // disabled: additional logic is required
            //if (e.NewValue is INotifyCollectionChanged collection3)
            //{
            //    collection3.CollectionChanged -= OnCollectionChanged;
            //    collection3.CollectionChanged += OnCollectionChanged;
            //}

            if (e.NewValue is INotifyPropertyChanged collection4)
            {
                collection4.PropertyChanged -= OnPropertyChanged; // Avoid double handler adding?
                collection4.PropertyChanged += OnPropertyChanged;
            }

            BuildTable();
        }

        //private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs _)
        //{
        //    BuildTable();
        //}

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs _)
        {
            BuildTable();
        }

        //public void OnTemplatesChanged(DependencyPropertyChangedEventArgs<ObservableCollection<ControlTemplate>> _)
        //{
        //    BuildTable();
        //}

        #endregion Public Methods
    }

    /// <summary>
    /// Grid table column's templates descriptor
    /// </summary>
    public class GridTableColumn : ColumnDefinition
    {
        #region Public Properties

        /// <summary>
        /// Item row cell template
        /// </summary>
        public ControlTemplate CellTemplate
        {
            get { return (ControlTemplate)GetValue(CellTemplateProperty); }
            set { SetValue(CellTemplateProperty, value); }
        }

        /// <summary>
        /// Item row header template
        /// </summary>
        public ControlTemplate HeaderTemplate
        {
            get { return (ControlTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        #endregion Public Properties

        #region Public Fields

        public static readonly DependencyProperty CellTemplateProperty = DP<GridTableColumn>.R(x => x.CellTemplate);

        public static readonly DependencyProperty HeaderTemplateProperty = DP<GridTableColumn>.R(x => x.HeaderTemplate);

        #endregion Public Fields
    }

    /// <summary>
    /// Grid table row's templates descriptor
    /// </summary>
    public class GridTableRow : RowDefinition
    {
        #region Public Properties

        /// <summary>
        /// Item row cell template
        /// </summary>
        public ControlTemplate CellTemplate
        {
            get { return (ControlTemplate)GetValue(CellTemplateProperty); }
            set { SetValue(CellTemplateProperty, value); }
        }

        /// <summary>
        /// Item row header template
        /// </summary>
        public ControlTemplate HeaderTemplate
        {
            get { return (ControlTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        #endregion Public Properties

        #region Public Fields

        public static readonly DependencyProperty CellTemplateProperty = DP<GridTableRow>.R(x => x.CellTemplate);

        public static readonly DependencyProperty HeaderTemplateProperty = DP<GridTableRow>.R(x => x.HeaderTemplate);

        #endregion Public Fields
    }
}
