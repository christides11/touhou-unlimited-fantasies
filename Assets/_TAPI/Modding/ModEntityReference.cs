namespace TAPI.Modding
{
    [System.Serializable]
    public class ModEntityReference : ModObjectReference
    {
        public string entityName;

        public ModEntityReference(string modIdentifier, string entityName)
        {
            this.modIdentifier = modIdentifier;
            this.entityName = entityName;
        }
    }
}