namespace TAPI.Modding
{
    [System.Serializable]
    public class ModStageReference : ModObjectReference
    {
        public string stageName;

        public ModStageReference(string modIdentifier, string stageName)
        {
            this.modIdentifier = modIdentifier;
            this.stageName = stageName;
        }

        public override string ToString()
        {
            return $"{modIdentifier}/{stageName}";
        }
    }
}