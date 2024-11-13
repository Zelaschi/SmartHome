import { Component, Input, input } from '@angular/core';
import HomeCreatedModel from '../../../../backend/services/Home/models/HomeCreatedModel';

@Component({
  selector: 'app-home-item',
  templateUrl: './home-item.component.html',
  styleUrl: './home-item.component.css'
})
export class HomeItemComponent {
  @Input() home: HomeCreatedModel | null = null;
  showMembers: boolean = false;

  ngOnInit(): void {
    console.log(this.home);
  }

  GetHomeMembers(homeId: string): void {
    this.showMembers = !this.showMembers;
  }
}
