import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

import { Assets } from '../../core/constants/assets.constants';

@Component({
  selector: 'app-game-container',
  imports: [RouterOutlet],
  templateUrl: './game-container.html',
  styleUrl: './game-container.scss',
})
export class GameContainerComponent {
  protected readonly background = Assets.Images.Backgrounds.CommonEmpty;
}
