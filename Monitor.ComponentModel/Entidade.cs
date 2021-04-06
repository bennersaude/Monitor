using System;
using System.ComponentModel.DataAnnotations;

namespace Monitor.ComponentModel
{
    [Serializable]
    public partial class Entidade
    {
        public Entidade()
        {
            Handle = 0;
        }

        [Key]
        public virtual long Handle { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as Entidade;

            if (other == null)
                return false;

            if (Handle == 0 && other.Handle == 0)
                return this == other;

            return Handle == other.Handle;
        }

        public override int GetHashCode()
        {
            if (Handle == 0)
                return base.GetHashCode();

            var stringRepresentation = string.Format("{0}#{1}", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, Handle);
            return stringRepresentation.GetHashCode();
        }

        public virtual T ClonarSuperficial<T>()
        {
            return (T) MemberwiseClone();
        }

        protected virtual T As<T>() where T : Entidade
        {
            return this as T;
        }

        public virtual Type TypeUnproxied
        {
            get { return GetType(); }
        }
    }
}
