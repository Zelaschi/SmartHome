import DropdownOption from "../../../../components/dropdown/models/DropdownOption";

export default interface HomePermissionStatus {
    loading?: true;
    homePermissions: Array<DropdownOption>;
    error?: string;
}