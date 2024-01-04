namespace Tvision2.Core.Engine;

    public class TvUiManager
    {
        class TvUiNode
        {
            public TvComponentTreeNode TreeNode { get; }
            public IEnumerable<TvComponentTreeNode> FlattenedSubTree { get; private set; }

            public TvUiNode(TvComponentTreeNode node)
            {
                TreeNode = node;
                FlattenedSubTree = Enumerable.Empty<TvComponentTreeNode>();
            }

            internal void Flatten()
            {
                FlattenedSubTree = TreeNode.SubTree().ToArray();
            }
        }

        private readonly TvComponentTree _tree;
        private readonly List<TvUiNode> _flattenedRoots;

        public TvComponentTree Components { get => _tree; }

        public TvUiManager()
        {
            _tree = new TvComponentTree();
            _flattenedRoots = new List<TvUiNode>();
            _tree.On().RootAdded.Do(n => FlattenRootNode(n));
            _tree.On().NodeAdded.Do(n => FlattenNode(n));
        }

        private void FlattenRootNode(TvComponentTreeNode newRoot)
        {
            var uiNode = new TvUiNode(newRoot);
            uiNode.Flatten();
            _flattenedRoots.Add(uiNode);
        }

        private void FlattenNode(TvComponentTreeNode newNode)
        {
            if (!newNode.IsRoot)
            {
                var root = newNode.Root();
                var uiNode = _flattenedRoots.SingleOrDefault(r => r.TreeNode == root);
                uiNode?.Flatten();
            }
        }

        internal void Draw(VirtualConsole console)
        {
            foreach (var root in _flattenedRoots)
            {
                foreach (var cmpNode in root.FlattenedSubTree)
                {
                    var component = cmpNode.Component;
                    
                    if (component.Metadata.GetAndCleanIsDrawPending()) {
                        var layer = component.Layer.LayerIndex switch
                        {
                            0 => console.BottomLayer,
                            Int32.MaxValue => console.TopLayer,
                            -1 => console.StandardLayer,
                            int v => console.Layer(v)
                        };
                        component.Draw(layer);
                    }
                }
            }
        }
        
        internal Task<bool> Update(bool forceDraws, TvConsoleEvents events)
        {
            var task = _tree.NewCycle()
                .ContinueWith(t =>
            {            
                var someDrawPending = false;
                var context = new UpdateContext(events);
                foreach (var root in _flattenedRoots)
                {
                    foreach (var cmpNode in root.FlattenedSubTree)
                    {
                        var result = cmpNode.Component.Update(context);
                        if (result == UpdateResult.Dirty || forceDraws)
                        {
                            cmpNode.Component.Metadata.SetDrawPending();
                        }
                        someDrawPending = someDrawPending || cmpNode.Component.Metadata.HasDrawPending();
                    }
                }
                return someDrawPending;
            });
            return task;
        }
    }