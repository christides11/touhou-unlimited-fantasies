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

        public ModObjectReference(string modIdentifier, string objectName)
        {
            this.modIdentifier = modIdentifier;
            this.objectName = objectName;
        }

        public override string ToString()
        {
            return $"{modIdentifier}/{objectName}";
        }
    }
}