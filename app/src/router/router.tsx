import { Route, Routes } from "react-router-dom";
import HomePage from "../pages/home";
import PermissionsPage from "../pages/permission/permissions";
import AddPermissionPage from "../pages/add-permission/add-permission";
import EditPermission from "../pages/edit-permission/edit-permission";
import PermissionTypesPage from "../pages/permission-type/permission-types";
import AddPermissionTypePage from "../pages/add-permission-type/add-permission-type";

const Router = () => {
    return (
        <Routes>
            <Route path="/" element={<HomePage/>}  />
            <Route path="/permissions" >
                <Route index element={<PermissionsPage/>} />
                <Route path=":id" element={<EditPermission/>} />
                <Route path="request" element={<AddPermissionPage/>} />
            </Route>
            <Route path="/permission-types" >
                <Route index element={<PermissionTypesPage/>} />
                <Route path="create" element={<AddPermissionTypePage/>} />
            </Route>
            <Route path="*" element={<HomePage/>} />
        </Routes>
    );
}

export default Router;
