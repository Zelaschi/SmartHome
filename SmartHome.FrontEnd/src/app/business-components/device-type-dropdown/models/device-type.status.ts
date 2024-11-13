import DropdownOption from "../../../components/dropdown/models/DropdownOption";

export default interface DeviceTypeStatus {
    loading?: true;
    deviceTypes: Array<DropdownOption>;
    error?: string;
}