using HospitalSystem_WebAPI_dotnet6.ViewModels;
using StackExchange.Redis;

namespace HospitalSystem_WebAPI_dotnet6.Utils {

    public static class RedisUtils {

        //扩展方法：在病人挂号或取消挂号后执行缓存清除
        public async static Task<bool> PatientRecordsRemover(this IConnectionMultiplexer _redis, PatientView patient) {

            var server = _redis.GetServer(_redis.GetEndPoints().First());
            var db = _redis.GetDatabase();

            var keys = server.Keys(pattern: $"*{patient.ID}*").ToArray();

            if (keys.Length > 0) {
                foreach (var key in keys) {
                    await db.KeyDeleteAsync(key);
                }
                return true;
            }
            return false;
        }

    }
}
