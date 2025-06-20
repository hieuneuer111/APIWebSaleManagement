namespace WebAPISalesManagement.Helpers
{
   
    public class ValidateHelper
    {
        public bool CheckGuid(string guidString)
        {
            if (!string.IsNullOrWhiteSpace(guidString))
            {
               Guid guid = Guid.Parse(guidString);
                if (guid == Guid.Empty)
                {
                    return false;
                }
                else { 
                    return true;
                }
            }  else
            {
                return false;
            }    
           
        }

    }
}
