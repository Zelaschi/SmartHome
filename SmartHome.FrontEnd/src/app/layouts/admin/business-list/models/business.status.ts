import { Business } from "../../../../../backend/services/Business/models/Business";

export default interface BusinessStatus {
    loading?: true;
    businesses: Array<Business>;
    error?: string;
}