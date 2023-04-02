namespace Cvijecara_Sanja_Tica_IT80_2019.CustomException
{
    public class IdIsAlreadyTakenException:Exception
    {
        public IdIsAlreadyTakenException()
        {

        }
        public IdIsAlreadyTakenException(string message):base(message)
        {

        }
    }
}
