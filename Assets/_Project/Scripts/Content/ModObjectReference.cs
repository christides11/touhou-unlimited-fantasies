namespace TUF.Modding
{
    [System.Serializable]
    public class ModObjectReference
    {
        public string modIdentifier;
        public string objectName;

        public ModObjectReference()
        {

        }

        public ModObjectReference(string modIdentifier, string objectIdentifier)
        {
            this.modIdentifier = modIdentifier;
            this.objectName = objectIdentifier;
        }

        public override string ToString()
        {
            return $"{modIdentifier}/{objectName}";
        }
    }
}