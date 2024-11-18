import HomePermissionOptions from "./HomePermissionsOptions";

export default interface HomePermissionStatus {
    loading?: boolean;
    homePermissions: Array<HomePermissionOptions>;
    error?: string;
}