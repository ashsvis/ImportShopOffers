using System;

namespace ImportShopOffers
{
    /// <summary>
    /// Interface of editor of object T
    /// </summary>
    public interface IEditor<T, E>
    {
        /// <summary>
        /// Start changing
        /// </summary>
        event EventHandler<ChangingEventArgs> StartChanging;

        /// <summary>
        /// Object was changed
        /// </summary>
        event EventHandler<EventArgs> Changed;

        /// <summary>
        /// Build editor interface for the object
        /// </summary>
        void Build(T columnInfo, E content);
    }

    public class ChangingEventArgs
    {
        public string ChangingName { get; set; }

        public ChangingEventArgs(string changingName)
        {
            this.ChangingName = changingName;
        }
    }
}
