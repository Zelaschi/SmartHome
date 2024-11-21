import { Business } from "../../../../../backend/services/Business/models/Business";

export default interface BusinessStatus {
    moreBusinesses:boolean;
    loading?: true;
    businesses: Array<Business>;
    error?: string;
}