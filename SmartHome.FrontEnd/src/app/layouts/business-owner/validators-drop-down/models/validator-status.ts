import DropdownOption from "../../../../components/dropdown/models/DropdownOption";

export default interface ValidatorStatus {
    loading?: true;
    validators: Array<DropdownOption>;
    error?: string;
}