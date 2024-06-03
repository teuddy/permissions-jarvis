import { configureStore } from "@reduxjs/toolkit";
import permissionsReducer from "./reducers/permissions.slice";
import { useDispatch } from "react-redux";
import permissionTypesReducer from "./reducers/permission-types.slice";

export const store = configureStore({
    reducer:{
        permission: permissionsReducer,
        permissionType: permissionTypesReducer
    },
});

export type RootState = ReturnType<typeof store.getState>;
export const useAppDispatch = () => useDispatch<typeof store.dispatch>();