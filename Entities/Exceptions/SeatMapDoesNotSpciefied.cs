namespace Entities.Exceptions;

public class SeatMapDoesNotSpciefied : BadRequestException
{
	public SeatMapDoesNotSpciefied()
		: base($"نقشه برای یکی از طبقات ارسال نشده است")
	{
	}
}
