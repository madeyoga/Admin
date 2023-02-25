namespace MyAdmin.Admin;

public record ModelAdminTypePair
{
	public required Type Model { get; init; }
	public required Type Admin { get; init; }
}
