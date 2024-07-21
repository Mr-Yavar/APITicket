namespace Entities.Exceptions;

public class NoImageAndPoster : BadRequestException
{
	public NoImageAndPoster()
		: base($"هیچ تصویری برای پوستر یا کارت بارگذاری نشده است!")
	{
	}
}
