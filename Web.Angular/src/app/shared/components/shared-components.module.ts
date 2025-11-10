import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ResponseMessageComponent } from './response-message/response-message.component';

@NgModule({
  declarations: [ResponseMessageComponent],
  imports: [CommonModule],
  exports: [ResponseMessageComponent]
})
export class SharedComponentsModule {}
