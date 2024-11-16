import { Component, Input } from '@angular/core';
import RoomResponseModel from '../../../../backend/services/Room/models/RoomResponseModel';

@Component({
  selector: 'app-room-item',
  templateUrl: './room-item.component.html',
  styleUrl: './room-item.component.css'
})
export class RoomItemComponent {
  @Input() room: RoomResponseModel | null = null;
  ngOnInit(): void {
    console.log(this.room);
  }
}
