import HomeCreatedModel from "../../../../../backend/services/Home/models/HomeCreatedModel";

export default interface HomeStatus {
    loading?: true;
    homes: Array<HomeCreatedModel>;
    error?: string;
}