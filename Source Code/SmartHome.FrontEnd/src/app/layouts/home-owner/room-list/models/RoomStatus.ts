import RoomCreatedModel from "../../../../../backend/services/Room/models/RoomCreatedModel";

export default interface RoomStatus {
    loading?: true;
    rooms: Array<RoomCreatedModel>;
    error?: string;
}