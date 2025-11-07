namespace PXPayBackend.Models

{
    public class TodoItem
    {
         private long _id;//私有變量 
         public long Id  
        {
            get { return _id; } //read 
            set { _id = value; } //Write
        }
        private string? _name;
        public string? Name
        {
            get { return _name; }
            set { _name = value; }
        }
                private bool _isComplete;
        public bool IsComplete
        {
            get { return _isComplete; }
            set { _isComplete = value; }
        }
    }
}
