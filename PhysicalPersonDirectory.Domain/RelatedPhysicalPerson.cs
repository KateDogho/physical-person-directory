namespace PhysicalPersonDirectory.Domain;

public class RelatedPhysicalPerson
{
    public RelationType RelationType { get; set; }

    public int TargetPersonId { get; set; }
    
    public int RelatedPersonId { get; set; }
    
    public virtual PhysicalPerson TargetPerson { get; set; } = new();
    
    public virtual PhysicalPerson RelatedPerson { get; set; } = new();
}