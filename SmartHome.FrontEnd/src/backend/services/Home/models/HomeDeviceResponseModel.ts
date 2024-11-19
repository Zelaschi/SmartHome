export default interface HomeMemberResponseModel {
    type: string;
    hardwardId: string;
    online: boolean;
    device: string;
    name: string;
    isOn?: boolean;
    open?: boolean;
    photos: Array<string>;
    room?: string;
  }