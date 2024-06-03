import { useSelector } from "react-redux";
import { RootState, useAppDispatch } from "../../state/store";
import { RequestState } from "../../models/action-state";
import Loader from "../../components/loader-message";
import PermissionsTable from "./permissions-table";
import ErrorMessage from "../../components/error-message";
import { useEffect } from "react";
import { getAllPermissions } from "../../state/reducers/permissions.slice";

const PermissionsBody = () => {
    const dispatch = useAppDispatch();
    const requestState = useSelector((state: RootState) => state.permission.permissionsListRequestState);

    useEffect(() => {
        dispatch(getAllPermissions());
    },[]);

    switch(requestState){
        case RequestState.PENDING:
            return <Loader />
        case RequestState.FAILED:
            return <ErrorMessage message="Something wrong happened while retrieving data" />
        case RequestState.SUCCESS:
            return <PermissionsTable />
    }

};

export default PermissionsBody;

