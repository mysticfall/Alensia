using System.Collections.Generic;
using System.Linq;
using Alensia.Core.Input;

namespace Alensia.Core.Control
{
    public abstract class AggregateControl : Control
    {
        public IReadOnlyCollection<IControl> Children => _children;

        public override bool Valid => base.Valid && _children.TrueForAll(c => c.Valid);

        private List<IControl> _children = Enumerable.Empty<IControl>().ToList();

        protected AggregateControl(IInputManager inputManager) : base(inputManager)
        {
        }

        protected abstract IEnumerable<IControl> CreateChildren();

        protected override void OnInitialized()
        {
            _children = CreateChildren().ToList();
            _children.ForEach(c => c.Initialize());

            base.OnInitialized();
        }

        protected override void OnDisposed()
        {
            _children.ForEach(c => c.Dispose());
            _children = Enumerable.Empty<IControl>().ToList();

            base.OnDisposed();
        }

        protected override void OnActivated()
        {
            _children.ForEach(c => c.Activate());

            base.OnActivated();
        }

        protected override void OnDeactivated()
        {
            _children.ForEach(c => c.Deactivate());

            base.OnDeactivated();
        }

        protected override void RegisterDefaultBindings()
        {
            base.RegisterDefaultBindings();

            foreach (var key in Bindings)
            {
                OnBindingChange(key);
            }
        }

        protected override ICollection<IBindingKey> PrepareBindings() => 
            _children.SelectMany(c => c.Bindings).ToList();
    }
}