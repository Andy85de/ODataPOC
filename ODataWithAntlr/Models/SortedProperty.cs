﻿using ODataWithSprache.Grammar;

namespace ODataWithSprache.Models;

/// <summary>
///     The sorted property to store the property name and
///     with which sort direction they should be ordered.
/// </summary>
public class SortedProperty
{
    /// <summary>
    ///     The name of the property that a sort should be made.
    /// </summary>
    public string PropertyName { get; set; }

    /// <summary>
    ///     The sort direction that should be apply.
    /// </summary>
    public SortDirection Sorted { set; get; }

    /// <summary>
    ///     Creates a object of type <see cref="SortedProperty" />.
    /// </summary>
    /// <param name="propertyName">The name of the property that a sort should be made.</param>
    /// <param name="sorted">The sort direction that should be apply.</param>
    public SortedProperty(string propertyName, SortDirection sorted)
    {
        PropertyName = propertyName;
        Sorted = sorted;
    }
}
