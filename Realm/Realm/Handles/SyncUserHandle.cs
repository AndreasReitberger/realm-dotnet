﻿////////////////////////////////////////////////////////////////////////////
//
// Copyright 2016 Realm Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
////////////////////////////////////////////////////////////////////////////

using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using MongoDB.Bson;
using Realms.Native;

namespace Realms.Sync
{
    internal class SyncUserHandle : StandaloneHandle
    {
        private static class NativeMethods
        {
#pragma warning disable IDE1006 // Naming Styles

            [DllImport(InteropConfig.DLL_NAME, EntryPoint = "realm_syncuser_get_id", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr get_user_id(SyncUserHandle user, IntPtr buffer, IntPtr buffer_length, out NativeException ex);

            [DllImport(InteropConfig.DLL_NAME, EntryPoint = "realm_syncuser_get_refresh_token", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr get_refresh_token(SyncUserHandle user, IntPtr buffer, IntPtr buffer_length, out NativeException ex);

            [DllImport(InteropConfig.DLL_NAME, EntryPoint = "realm_syncuser_get_access_token", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr get_access_token(SyncUserHandle user, IntPtr buffer, IntPtr buffer_length, out NativeException ex);

            [DllImport(InteropConfig.DLL_NAME, EntryPoint = "realm_syncuser_get_device_id", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr get_device_id(SyncUserHandle user, IntPtr buffer, IntPtr buffer_length, out NativeException ex);

            [DllImport(InteropConfig.DLL_NAME, EntryPoint = "realm_syncuser_get_state", CallingConvention = CallingConvention.Cdecl)]
            public static extern UserState get_state(SyncUserHandle user, out NativeException ex);

            [DllImport(InteropConfig.DLL_NAME, EntryPoint = "realm_syncuser_get_auth_provider", CallingConvention = CallingConvention.Cdecl)]
            public static extern Credentials.AuthProvider get_auth_provider(SyncUserHandle user, out NativeException ex);

            [DllImport(InteropConfig.DLL_NAME, EntryPoint = "realm_syncuser_get_profile_data", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr get_profile_data(SyncUserHandle user, UserProfileField field,
                IntPtr buffer, IntPtr buffer_length, [MarshalAs(UnmanagedType.U1)] out bool isNull,
                out NativeException ex);

            [DllImport(InteropConfig.DLL_NAME, EntryPoint = "realm_syncuser_get_custom_data", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr get_custom_data(SyncUserHandle user, IntPtr buffer, IntPtr buffer_length,
                [MarshalAs(UnmanagedType.U1)] out bool isNull, out NativeException ex);

            [DllImport(InteropConfig.DLL_NAME, EntryPoint = "realm_syncuser_get_app", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr get_app(SyncUserHandle user, out NativeException ex);

            [DllImport(InteropConfig.DLL_NAME, EntryPoint = "realm_syncuser_log_out", CallingConvention = CallingConvention.Cdecl)]
            public static extern void log_out(SyncUserHandle user, out NativeException ex);

            [DllImport(InteropConfig.DLL_NAME, EntryPoint = "realm_syncuser_refresh_custom_data", CallingConvention = CallingConvention.Cdecl)]
            public static extern void refresh_custom_data(SyncUserHandle user, IntPtr tcs_ptr, out NativeException ex);

            [DllImport(InteropConfig.DLL_NAME, EntryPoint = "realm_syncuser_destroy", CallingConvention = CallingConvention.Cdecl)]
            public static extern void destroy(IntPtr syncuserHandle);

            [DllImport(InteropConfig.DLL_NAME, EntryPoint = "realm_syncuser_call_function", CallingConvention = CallingConvention.Cdecl)]
            public static extern void call_function(SyncUserHandle handle, AppHandle app,
                [MarshalAs(UnmanagedType.LPWStr)] string function_name, IntPtr function_name_len,
                [MarshalAs(UnmanagedType.LPWStr)] string args, IntPtr args_len,
                [MarshalAs(UnmanagedType.LPWStr)] string service_name, IntPtr service_name_len,
                IntPtr tcs_ptr, out NativeException ex);

            [DllImport(InteropConfig.DLL_NAME, EntryPoint = "realm_syncuser_link_credentials", CallingConvention = CallingConvention.Cdecl)]
            public static extern void link_credentials(SyncUserHandle handle, AppHandle app, Native.Credentials credentials, IntPtr tcs_ptr, out NativeException ex);

            [DllImport(InteropConfig.DLL_NAME, EntryPoint = "realm_syncuser_get_serialized_identities", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr get_identities(SyncUserHandle handle, IntPtr buffer, IntPtr bufsize, out NativeException ex);

            #region Push

            [DllImport(InteropConfig.DLL_NAME, EntryPoint = "realm_syncuser_push_register", CallingConvention = CallingConvention.Cdecl)]
            public static extern void push_register(SyncUserHandle handle, AppHandle app,
                [MarshalAs(UnmanagedType.LPWStr)] string service, IntPtr service_len,
                [MarshalAs(UnmanagedType.LPWStr)] string token, IntPtr token_len,
                IntPtr tcs_ptr, out NativeException ex);

            [DllImport(InteropConfig.DLL_NAME, EntryPoint = "realm_syncuser_push_deregister", CallingConvention = CallingConvention.Cdecl)]
            public static extern void push_deregister(SyncUserHandle handle, AppHandle app,
                [MarshalAs(UnmanagedType.LPWStr)] string service, IntPtr service_len,
                IntPtr tcs_ptr, out NativeException ex);

            #endregion

            #region Api Keys

            [DllImport(InteropConfig.DLL_NAME, EntryPoint = "realm_syncuser_api_key_create", CallingConvention = CallingConvention.Cdecl)]
            public static extern void create_api_key(SyncUserHandle handle, AppHandle app,
                [MarshalAs(UnmanagedType.LPWStr)] string name, IntPtr name_len,
                IntPtr tcs_ptr, out NativeException ex);

            [DllImport(InteropConfig.DLL_NAME, EntryPoint = "realm_syncuser_api_key_fetch", CallingConvention = CallingConvention.Cdecl)]
            public static extern void fetch_api_key(SyncUserHandle handle, AppHandle app, PrimitiveValue id, IntPtr tcs_ptr, out NativeException ex);

            [DllImport(InteropConfig.DLL_NAME, EntryPoint = "realm_syncuser_api_key_fetch_all", CallingConvention = CallingConvention.Cdecl)]
            public static extern void fetch_api_keys(SyncUserHandle handle, AppHandle app, IntPtr tcs_ptr, out NativeException ex);

            [DllImport(InteropConfig.DLL_NAME, EntryPoint = "realm_syncuser_api_key_delete", CallingConvention = CallingConvention.Cdecl)]
            public static extern void delete_api_key(SyncUserHandle handle, AppHandle app, PrimitiveValue id, IntPtr tcs_ptr, out NativeException ex);

            [DllImport(InteropConfig.DLL_NAME, EntryPoint = "realm_syncuser_api_key_disable", CallingConvention = CallingConvention.Cdecl)]
            public static extern void disable_api_key(SyncUserHandle handle, AppHandle app, PrimitiveValue id, IntPtr tcs_ptr, out NativeException ex);

            [DllImport(InteropConfig.DLL_NAME, EntryPoint = "realm_syncuser_api_key_enable", CallingConvention = CallingConvention.Cdecl)]
            public static extern void enable_api_key(SyncUserHandle handle, AppHandle app, PrimitiveValue id, IntPtr tcs_ptr, out NativeException ex);

            #endregion

#pragma warning restore IDE1006 // Naming Styles
        }

        [Preserve]
        public SyncUserHandle(IntPtr handle) : base(handle)
        {
        }

        public string GetUserId()
        {
            return MarshalHelpers.GetString((IntPtr buffer, IntPtr length, out bool isNull, out NativeException ex) =>
            {
                isNull = false;
                return NativeMethods.get_user_id(this, buffer, length, out ex);
            });
        }

        public string GetRefreshToken()
        {
            return MarshalHelpers.GetString((IntPtr buffer, IntPtr length, out bool isNull, out NativeException ex) =>
            {
                isNull = false;
                return NativeMethods.get_refresh_token(this, buffer, length, out ex);
            });
        }

        public string GetAccessToken()
        {
            return MarshalHelpers.GetString((IntPtr buffer, IntPtr length, out bool isNull, out NativeException ex) =>
            {
                isNull = false;
                return NativeMethods.get_access_token(this, buffer, length, out ex);
            });
        }

        public string GetDeviceId()
        {
            return MarshalHelpers.GetString((IntPtr buffer, IntPtr length, out bool isNull, out NativeException ex) =>
            {
                isNull = false;
                return NativeMethods.get_device_id(this, buffer, length, out ex);
            });
        }

        public UserState GetState()
        {
            var result = NativeMethods.get_state(this, out var ex);
            ex.ThrowIfNecessary();
            return result;
        }

        public Credentials.AuthProvider GetProvider()
        {
            var result = NativeMethods.get_auth_provider(this, out var ex);
            ex.ThrowIfNecessary();
            return result;
        }

        public bool TryGetApp(out AppHandle appHandle)
        {
            var result = NativeMethods.get_app(this, out var ex);
            ex.ThrowIfNecessary();

            if (result == IntPtr.Zero)
            {
                appHandle = null;
                return false;
            }

            appHandle = new AppHandle(result);
            return true;
        }

        public void LogOut()
        {
            NativeMethods.log_out(this, out var ex);
            ex.ThrowIfNecessary();
        }

        public async Task RefreshCustomDataAsync()
        {
            var tcs = new TaskCompletionSource<object>();
            var tcsHandle = GCHandle.Alloc(tcs);
            try
            {
                NativeMethods.refresh_custom_data(this, GCHandle.ToIntPtr(tcsHandle), out var ex);
                ex.ThrowIfNecessary();

                await tcs.Task;
            }
            finally
            {
                tcsHandle.Free();
            }
        }

        public string GetProfileData(UserProfileField field)
        {
            return MarshalHelpers.GetString((IntPtr buffer, IntPtr length, out bool isNull, out NativeException ex) =>
            {
                return NativeMethods.get_profile_data(this, field, buffer, length, out isNull, out ex);
            });
        }

        public string GetCustomData()
        {
            return MarshalHelpers.GetString((IntPtr buffer, IntPtr length, out bool isNull, out NativeException ex) =>
            {
                return NativeMethods.get_custom_data(this, buffer, length, out isNull, out ex);
            });
        }

        public async Task<string> CallFunctionAsync(AppHandle app, string name, string args, string service)
        {
            var tcs = new TaskCompletionSource<string>();
            var tcsHandle = GCHandle.Alloc(tcs);

            try
            {
                NativeMethods.call_function(this, app, name, name.IntPtrLength(), args, args.IntPtrLength(), service, service.IntPtrLength(), GCHandle.ToIntPtr(tcsHandle), out var ex);
                ex.ThrowIfNecessary();

                return await tcs.Task;
            }
            finally
            {
                tcsHandle.Free();
            }
        }

        public async Task<SyncUserHandle> LinkCredentialsAsync(AppHandle app, Native.Credentials credentials)
        {
            var tcs = new TaskCompletionSource<SyncUserHandle>();
            var tcsHandle = GCHandle.Alloc(tcs);
            try
            {
                NativeMethods.link_credentials(this, app, credentials, GCHandle.ToIntPtr(tcsHandle), out var ex);
                ex.ThrowIfNecessary();

                return await tcs.Task;
            }
            finally
            {
                tcsHandle.Free();
            }
        }

        public string GetIdentities()
        {
            return MarshalHelpers.GetString((IntPtr buffer, IntPtr length, out bool isNull, out NativeException ex) =>
            {
                isNull = false;
                return NativeMethods.get_identities(this, buffer, length, out ex);
            });
        }

        #region Push

        public void RegisterPushToken(AppHandle app, string service, string token, TaskCompletionSource<object> tcs)
        {
            var tcsHandle = GCHandle.Alloc(tcs);

            NativeMethods.push_register(this, app, service, (IntPtr)service.Length, token, (IntPtr)token.Length, GCHandle.ToIntPtr(tcsHandle), out var ex);
            ex.ThrowIfNecessary();
        }

        public void DeregisterPushToken(AppHandle app, string service, TaskCompletionSource<object> tcs)
        {
            var tcsHandle = GCHandle.Alloc(tcs);

            NativeMethods.push_deregister(this, app, service, (IntPtr)service.Length, GCHandle.ToIntPtr(tcsHandle), out var ex);
            ex.ThrowIfNecessary();
        }

        #endregion

        #region Api Keys

        public async Task<ApiKey> CreateApiKeyAsync(AppHandle app, string name)
        {
            var tcs = new TaskCompletionSource<ApiKey[]>();
            var tcsHandle = GCHandle.Alloc(tcs);

            try
            {
                NativeMethods.create_api_key(this, app, name, (IntPtr)name.Length, GCHandle.ToIntPtr(tcsHandle), out var ex);
                ex.ThrowIfNecessary();

                var result = await tcs.Task;

                Debug.Assert(result.Length == 1, "The result of Create should be exactly 1 ApiKey.");

                return result.Single();
            }
            finally
            {
                tcsHandle.Free();
            }
        }

        public async Task<ApiKey> FetchApiKeyAsync(AppHandle app, ObjectId id)
        {
            var tcs = new TaskCompletionSource<ApiKey[]>();
            var tcsHandle = GCHandle.Alloc(tcs);

            try
            {
                var primitiveId = PrimitiveValue.ObjectId(id);
                NativeMethods.fetch_api_key(this, app, primitiveId, GCHandle.ToIntPtr(tcsHandle), out var ex);
                ex.ThrowIfNecessary();

                var result = await tcs.Task;

                Debug.Assert(result == null || result.Length <= 1, "The result of the fetch operation should be either null, or an array of 0 or 1 elements.");

                return result == null || result.Length == 0 ? null : result.Single();
            }
            finally
            {
                tcsHandle.Free();
            }
        }

        public async Task<ApiKey[]> FetchAllApiKeysAsync(AppHandle app)
        {
            var tcs = new TaskCompletionSource<ApiKey[]>();
            var tcsHandle = GCHandle.Alloc(tcs);

            try
            {
                NativeMethods.fetch_api_keys(this, app, GCHandle.ToIntPtr(tcsHandle), out var ex);
                ex.ThrowIfNecessary();

                return await tcs.Task;
            }
            finally
            {
                tcsHandle.Free();
            }
        }

        public async Task DeleteApiKeyAsync(AppHandle app, ObjectId id)
        {
            var tcs = new TaskCompletionSource<object>();
            var tcsHandle = GCHandle.Alloc(tcs);
            try
            {
                var primitiveId = PrimitiveValue.ObjectId(id);
                NativeMethods.delete_api_key(this, app, primitiveId, GCHandle.ToIntPtr(tcsHandle), out var ex);
                ex.ThrowIfNecessary();

                await tcs.Task;
            }
            finally
            {
                tcsHandle.Free();
            }
        }

        public async Task DisableApiKeyAsync(AppHandle app, ObjectId id)
        {
            var tcs = new TaskCompletionSource<object>();
            var tcsHandle = GCHandle.Alloc(tcs);

            try
            {
                var primitiveId = PrimitiveValue.ObjectId(id);
                NativeMethods.disable_api_key(this, app, primitiveId, GCHandle.ToIntPtr(tcsHandle), out var ex);
                ex.ThrowIfNecessary();

                await tcs.Task;
            }
            finally
            {
                tcsHandle.Free();
            }
        }

        public async Task EnableApiKeyAsync(AppHandle app, ObjectId id)
        {
            var tcs = new TaskCompletionSource<object>();
            var tcsHandle = GCHandle.Alloc(tcs);

            try
            {
                var primitiveId = PrimitiveValue.ObjectId(id);
                NativeMethods.enable_api_key(this, app, primitiveId, GCHandle.ToIntPtr(tcsHandle), out var ex);
                ex.ThrowIfNecessary();

                await tcs.Task;
            }
            finally
            {
                tcsHandle.Free();
            }
        }

        #endregion

        protected override void Unbind()
        {
            NativeMethods.destroy(handle);
        }
    }
}
