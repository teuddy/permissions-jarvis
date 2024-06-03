import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import axios from 'axios';
import { Permission, PermissionRequest, UpdatePermissionRequest } from '../../models/permission';
import { RequestState, ToastConfig } from '../../models/action-state';

import { toast } from 'react-toastify';

const client = axios.create({ 
    baseURL: import.meta.env.VITE_API_URL, 
    headers: { 'Content-Type': 'application/json' }, 
    withCredentials: false 
});

export const getAllPermissions = createAsyncThunk('permissions/getAllPermissions', async () => {
    const response = await client.get('/permission');
    return response.data;
});

export const requestPermission = createAsyncThunk('permissions/requestPermission', async (permission: PermissionRequest) => {
    await client.post('/permission', permission);
});

export const getPermissionById = createAsyncThunk('permissions/getPermissionById', async (id: number) => {
    const response = await client.get(`/permission/${id}`);
    return response.data;
});

export const updatePermission = createAsyncThunk('permissions/updatePermission', async (permission: UpdatePermissionRequest) => {
    await client.patch(`/permission/${permission.id}`, permission);
});


const permissionsSlice = createSlice({
    name: 'permissions',
    initialState: {
        permissions: [] as Permission[],
        permission: {} as UpdatePermissionRequest,
        permissionsListRequestState: RequestState.IDLE,
        permissionRequestState: RequestState.IDLE,
    },
    reducers:{},
    extraReducers: (builder) => {

        builder.addCase(getAllPermissions.pending, (state) => {
            state.permissions = [];
            state.permissionsListRequestState = RequestState.PENDING;
        });

        builder.addCase(getAllPermissions.fulfilled, (state, action) => {
            state.permissions = action.payload;
            state.permissionsListRequestState = RequestState.SUCCESS;
        });

        builder.addCase(getAllPermissions.rejected, (state, action) => {
            state.permissions = [];
            state.permissionsListRequestState = RequestState.FAILED;
            toast.error('oh snap! something went wrong...', ToastConfig);
            console.error(action.error);
        });

        builder.addCase(requestPermission.pending, (state) => {
            state.permissionRequestState = RequestState.PENDING;
        });

        builder.addCase(requestPermission.fulfilled, (state, _) => {
            state.permissionRequestState = RequestState.SUCCESS;
            toast.success('Permission requested successfuly', {
                ...ToastConfig,
                type: 'success',
                isLoading: false,
              });
        });

        builder.addCase(requestPermission.rejected, (state, action) => {
            state.permissionRequestState = RequestState.FAILED;
            toast.error('oh snap! something went wrong...', ToastConfig);
            console.error(action.error);
        });

        builder.addCase(updatePermission.pending, (state) => {
            state.permissionRequestState = RequestState.PENDING;
        });

        builder.addCase(updatePermission.fulfilled, (state, _) => {
            state.permissionRequestState = RequestState.SUCCESS;
            toast.success('Permission updated successfuly', {
                ...ToastConfig,
                type: 'success',
                isLoading: false,
              }); 
        });

        builder.addCase(updatePermission.rejected, (state, action) => {
            state.permissionRequestState = RequestState.FAILED;
            toast.error('oh snap! something went wrong...', ToastConfig);
            console.error(action.error);
        });
    }
});

export default permissionsSlice.reducer;