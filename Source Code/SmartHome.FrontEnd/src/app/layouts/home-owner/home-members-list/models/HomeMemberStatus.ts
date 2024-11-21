import HomeMemberResponseModel from "../../../../../backend/services/Home/models/HomeMemberResponseModel";

export default interface HomeStatus {
    loading?: true;
    homeMembers: Array<HomeMemberResponseModel>;
    error?: string;
}