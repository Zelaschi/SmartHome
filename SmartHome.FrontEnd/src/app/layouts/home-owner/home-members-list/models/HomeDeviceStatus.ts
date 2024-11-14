import HomeDeviceResponseModel from "../../../../../backend/services/Home/models/HomeDeviceResponseModel";

export default interface HomeDeviceStatus {
    loading?: true;
    homeDevices: Array<HomeDeviceResponseModel>;
    error?: string;
}