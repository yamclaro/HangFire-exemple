namespace HangFire2.Models
{
    public interface IjobTestService
    {
        void FireAndForgetJob();
        void ReccuringJob();
        void DelayedJob();
        void ContinuationJob();
         
    }
}