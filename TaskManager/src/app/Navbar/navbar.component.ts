import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormModalComponent } from '../TaskForm/form.component';
import { MatDialog } from '@angular/material/dialog';
@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css'],
})
export class NavbarComponent {
  isMenuOpen = false;
  constructor(private dialog: MatDialog) {}
  toggleMenu(): void {
    this.isMenuOpen = !this.isMenuOpen;
  }
  openFormModal() {
    const dialogRef = this.dialog.open(FormModalComponent, {
      width: '400px',
    });
    dialogRef.afterClosed().subscribe((result) => {
      console.log('Form result:', result);
    });
  }
}
