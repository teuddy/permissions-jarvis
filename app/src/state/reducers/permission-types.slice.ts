import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import axios from 'axios';
import { PermissionType, PermissionTypeRequest } from '../../models/permission';
import { RequestState } from '../../models/action-state';

const client = axios.create({ 
    baseURL: import.meta.env.VITE_API_URL, 
    headers: { 'Content-Type': 'application/json' }, 
    withCredentials: false 
});

export const getAllPermissionTypes = createAsyncThunk('permission-types/getAllPermissionTypes', async () => {
    const response = await client.get('/permissiontype');
    return response.data;
});

export const createPermissionType = createAsyncThunk('permission-types/createPermissionType', async (permissionType: PermissionTypeRequest) => {
    await client.post('/permissiontype', permissionType);
});

const permissionTypeSlice = createSlice({
    name: 'permission-types',
    initialState: {
        permissionTypes: [] as PermissionType[],
        permissionTypesRequestState: RequestState.IDLE,
    },
    reducers:{},
    extraReducers: (builder) => {

        builder.addCase(getAllPermissionTypes.pending, (state) => {
            state.permissionTypes = [];
            state.permissionTypesRequestState = RequestState.PENDING;
        });

        builder.addCase(getAllPermissionTypes.fulfilled, (state, action) => {
            state.permissionTypes = action.payload;
            state.permissionTypesRequestState = RequestState.SUCCESS;
        });

        builder.addCase(getAllPermissionTypes.rejected, (state, action) => {
            state.permissionTypes = [];
            state.permissionTypesRequestState = RequestState.FAILED;
            console.error(action.error);
        });
    }
});

export default permissionTypeSlice.reducer;