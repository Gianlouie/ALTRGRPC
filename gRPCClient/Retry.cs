﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ALTRgRPCClient
{
    public static class Retry
    {
        public static async Task<T> DoAsync<T>(Func<Task<T>> action,
                    Func<T, bool> validateResult = null,
                    int maxRetries = 10, int maxDelayMilliseconds = 2000, int delayMilliseconds = 200)

        {
            var backoff = new ExponentialBackoff(delayMilliseconds, maxDelayMilliseconds);

            var exceptions = new List<Exception>();

            for (var retry = 0; retry < maxRetries; retry++)
            {
                try
                {
                    var result = await action()
                        .ConfigureAwait(false);
                    var isValid = validateResult?.Invoke(result);
                    if (isValid.HasValue && isValid.Value)
                        return result;
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                    await backoff.Delay()
                        .ConfigureAwait(false);
                }
            }

            throw new AggregateException(exceptions);
        }

        private struct ExponentialBackoff
        {
            private readonly int _delayMilliseconds;
            private readonly int _maxDelayMilliseconds;
            private int _retries;
            private int _pow;

            public ExponentialBackoff(int delayMilliseconds, int maxDelayMilliseconds)
            {
                _delayMilliseconds = delayMilliseconds;
                _maxDelayMilliseconds = maxDelayMilliseconds;
                _retries = 0;
                _pow = 1;
            }

            public Task Delay()
            {
                ++_retries;
                if (_retries < 31)
                {
                    _pow = _pow << 1; // m_pow = Pow(2, m_retries - 1)
                }

                var delay = Math.Min(_delayMilliseconds * (_pow - 1) / 2, _maxDelayMilliseconds);
                return Task.Delay(delay);
            }
        }
    }
}
