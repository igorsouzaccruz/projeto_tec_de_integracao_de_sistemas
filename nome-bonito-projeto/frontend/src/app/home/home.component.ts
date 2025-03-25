import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NomeService } from '../services/nome.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {
  nome: string = '';
  resultado: string = '';

  constructor(private nomeService: NomeService) {}

  verificarNome(): void {
    this.nomeService.verificarNome(this.nome).subscribe(() => {
      setTimeout(() => {
        this.nomeService.obterResultado().subscribe((res) => {
          this.resultado = res
            ? 'Sim, seu nome é bonito! 😊'
            : 'Não, seu nome não é bonito. 😞';
        });
      }, 2000);
    });
  }
}
