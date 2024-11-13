import { Component, Input, input } from '@angular/core';
import HomeCreatedModel from '../../../../backend/services/Home/models/HomeCreatedModel';

@Component({
  selector: 'app-home-item',
  templateUrl: './home-item.component.html',
  styleUrl: './home-item.component.css'
})
export class HomeItemComponent {
  @Input() home: HomeCreatedModel | null = null;

  ngOnInit(): void {
    console.log(this.home);
  }
}
