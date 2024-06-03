import { useSelector } from "react-redux";
import { RootState, useAppDispatch } from "../../state/store";
import { RequestState } from "../../models/action-state";
import Loader from "../../components/loader-message";
import PermissionTypeTable from "./permission-type-table";
import ErrorMessage from "../../components/error-message";
import { useEffect } from "react";
import { getAllPermissionTypes } from "../../state/reducers/permission-types.slice";

const PermissionTypeBody = () => {
    const dispatch = useAppDispatch();
    const requestState = useSelector((state: RootState) => state.permissionType.permissionTypesRequestState);

    useEffect(() => {
        dispatch(getAllPermissionTypes());
    },[]);

    switch(requestState){
        case RequestState.PENDING:
            return <Loader />
        case RequestState.FAILED:
            return <ErrorMessage message="Something wrong happened while retrieving data" />
        case RequestState.SUCCESS:
            return <PermissionTypeTable />
    }

};

export default PermissionTypeBody;

