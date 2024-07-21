namespace Entities.Exceptions;

public class NoTicketHasBeenSpciefied : BadRequestException
{
	public NoTicketHasBeenSpciefied()
		: base($"هیچ بلیتی برای رویداد در نظر گرفته نشده است")
	{
	}
}
