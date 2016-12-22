using System;

namespace SFA.DAS.Data.Pipeline.Helpers
{
    public static class TransformationExtensions
    {
        public static PipelineResult<TO> Transform<T,TO>(
            this PipelineResult<T> result, Func<T,TO> transform, string message)
        {
            return result.Step(x => Result.Win(transform(x),message));
        }
    }
}