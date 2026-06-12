import { Component, input } from '@angular/core';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatButtonModule } from '@angular/material/button';
/**
 * @title Card with footer
 */
@Component({
  selector: 'card-footer-example',
  templateUrl: './card.component.html',
  styleUrl: './card.component.css',
  imports: [MatCardModule, MatChipsModule, MatProgressBarModule, MatButtonModule],
})
export class CardFooterExample {
  taskId = input();
  title = input();
  description = input();
  IsCompleted = input();
  CreatedAt = input();
}
