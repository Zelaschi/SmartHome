import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RoomFormComponent } from './room-form/room-form.component';

const routes: Routes = [
  {
    path: '',
    component: RoomFormComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RoomRoutingModule { }
