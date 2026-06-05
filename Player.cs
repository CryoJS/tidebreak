using System;
using GameUtility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameGum;
using Tidebreak;
using Tidebreak.Screens;

class Player
{
    // Store player physics constants (pixels/seconds)
    private const float GRAVITY = 8000f;
    private const float MAX_SPEED = 100000f;
    private const float RUN_SPEED = 700f;
    private const float SWIM_SPEED = WATER_RESISTANCE + RUN_SPEED;
    private const float ZIPLINE_SPEED = 1200f;
    private const float JUMP_SPEED = -2500f;
    private const float CLIMB_SPEED = 800f;
    private const float DIVE_SPEED = 2800f;
    private const float MAX_FALL_SPEED = 3000f;
    private const float AIR_RESISTANCE = 13000f;
    private const float WATER_RESISTANCE = 1000f;
    private const float WALL_JUMP_SPEED = 3200f;

    private const float DEFAULT_THRESHOLD = 50f;
    private const float FALL_THRESHOLD = 0.9f * MAX_FALL_SPEED;
    private const float FALL_DEATH_THRESHOLD = 4; // tiles
    private const float CLIMB_DOWN_BUFFER = 200; // ms
    private const float COYOTE_MAX = 0.1f; // seconds
    private const float WATER_LOWER_CENTER_AMOUNT = 0.25f;
    private const int X_OVERLAP_THRESHOLD = Game1.TILE_SIZE;

    // Store player animation constants
    private const int FRAME_DURATION = 100;     // Duration of each animation frame in milliseconds
    private const int ANIM_SIZE = 128;          // Pixel size (player is in the center of a 128x128 frame)
    private const int PLAYER_WIDTH = 10;        // Actual width of the player sprite (for collisions)
    private const int PLAYER_HEIGHT = 30;       // Actual height of the player sprite (for collisions)
    private const int TILE_CHECK_SIZE = 2;      // Farthest # of tiles away from player to check for collisions
    private const int TRUE_HEIGHT = PLAYER_HEIGHT * Game1.PIXEL_SCALE;
    private const float HEIGHT_SHRINK_FACTOR = 0.3f;

    // Store player attribute constants
    public const int MAX_OXYGEN = 100;
    private const float INHALE_SPEED = 30f;

    // Store possible player animation states
    private const int ANIM_STATE_AMOUNT = 11;
    private enum animStates
    {
        Idle,
        Running,
        Landing,
        Jumping,
        Falling,
        Swimming,
        Sliding,
        Latching,
        Climbing,
        Hanging,
        Dying
    }

    // Store player movement data
    private animStates state = animStates.Idle;
    public Rectangle rec = new Rectangle(0, 0, PLAYER_WIDTH * Game1.PIXEL_SCALE, PLAYER_HEIGHT * Game1.PIXEL_SCALE);
    public Vector2 pos = new Vector2(0, 0);
    public Vector2 prevPos;

    public Vector2 vel = new Vector2(0, 0); // Per second
    private SpriteEffects direction = Animation.FLIP_NONE;
    private int moveInput;
    private float coyoteTime = 0;

    public bool IsDead { get; private set; }
    private bool isGrounded; // any ground (ie. ladders)
    private bool onGround; // specifically platforms
    private bool ceilingAbove;
    private bool isLatching;
    private bool isClimbing;
    private bool onZipline;
    private Zipline curZipline;

    private bool inWater;
    private bool prevInWater;
    private bool isSliding;
    private bool isPrevSliding;

    // Store player following camera
    public Camera Camera { get; private set; } = new Camera(Game1._graphics.GraphicsDevice.Viewport);

    // Store player attribute data
    public float Oxygen { get; private set; } = MAX_OXYGEN;

    // Store the buttons the player needs to get
    public Button NextButton { get; set; }
    public BSTree<Button> Buttons { get; set; }

    // Store button indicator constants
    ButtonIndicator btnIndic = new ButtonIndicator();

    // Store variables for screen glow when getting a button
    private float btnGlow = 0; // Opacity in range [0, 1]
    private readonly float btnGlowSpeed = 1;

    // Store ladder climb down timer
    Timer climbDownTimer = new Timer(CLIMB_DOWN_BUFFER, false);

    // Store player collision rectangles
    private Rectangle top;
    private Rectangle bottom;
    private Rectangle left;
    private Rectangle right;

    // Store player animation data
    private static Texture2D[] imgs = new Texture2D[ANIM_STATE_AMOUNT];
    private static Animation[] anims = new Animation[ANIM_STATE_AMOUNT];

    public Player(ContentManager content)
    {
        // Load player animation spreadsheets
        foreach (animStates animState in Enum.GetValues(typeof(animStates)))
        {
            imgs[(int)animState] = content.Load<Texture2D>($"Images/Sprites/Player/Hero{animState}");
        }

        // Create player animations
        anims[(int)animStates.Idle] = new Animation(imgs[(int)animStates.Idle], 10, 1, 10, 0, Animation.NO_IDLE, Animation.ANIMATE_FOREVER, 10 * FRAME_DURATION, pos, Game1.PIXEL_SCALE, Game1.PIXEL_SCALE, true, "PlayerIdle");
        anims[(int)animStates.Running] = new Animation(imgs[(int)animStates.Running], 10, 1, 10, 0, Animation.NO_IDLE, Animation.ANIMATE_FOREVER, 10 * FRAME_DURATION, pos, Game1.PIXEL_SCALE, Game1.PIXEL_SCALE, true, "PlayerRun");
        anims[(int)animStates.Landing] = new Animation(imgs[(int)animStates.Landing], 5, 1, 5, 0, Animation.NO_IDLE, 1, 5 * FRAME_DURATION, pos, Game1.PIXEL_SCALE, Game1.PIXEL_SCALE, false, "PlayerLand");
        anims[(int)animStates.Jumping] = new Animation(imgs[(int)animStates.Jumping], 6, 1, 6, 0, 5, 1, 6 * FRAME_DURATION, pos, Game1.PIXEL_SCALE, Game1.PIXEL_SCALE, false, "PlayerJump");
        anims[(int)animStates.Falling] = new Animation(imgs[(int)animStates.Falling], 3, 1, 3, 0, Animation.NO_IDLE, Animation.ANIMATE_FOREVER, 3 * FRAME_DURATION, pos, Game1.PIXEL_SCALE, Game1.PIXEL_SCALE, true, "PlayerFall");
        anims[(int)animStates.Swimming] = new Animation(imgs[(int)animStates.Swimming], 6, 1, 6, 0, Animation.NO_IDLE, Animation.ANIMATE_FOREVER, 6 * FRAME_DURATION, pos, Game1.PIXEL_SCALE, Game1.PIXEL_SCALE, true, "PlayerSwim");
        anims[(int)animStates.Sliding] = new Animation(imgs[(int)animStates.Sliding], 6, 1, 6, 2, Animation.NO_IDLE, Animation.ANIMATE_FOREVER, 6 * FRAME_DURATION, pos, Game1.PIXEL_SCALE, Game1.PIXEL_SCALE, false, "PlayerSlide");
        anims[(int)animStates.Latching] = new Animation(imgs[(int)animStates.Latching], 4, 1, 4, 0, Animation.NO_IDLE, Animation.ANIMATE_FOREVER, 8 * FRAME_DURATION, pos, Game1.PIXEL_SCALE, Game1.PIXEL_SCALE, false, "PlayerWallJump");
        anims[(int)animStates.Climbing] = new Animation(imgs[(int)animStates.Climbing], 4, 1, 4, 0, Animation.NO_IDLE, Animation.ANIMATE_FOREVER, 4 * FRAME_DURATION, pos, Game1.PIXEL_SCALE, Game1.PIXEL_SCALE, true, "PlayerClimb");
        anims[(int)animStates.Hanging] = new Animation(imgs[(int)animStates.Hanging], 3, 1, 3, 0, Animation.NO_IDLE, Animation.ANIMATE_FOREVER, 3 * FRAME_DURATION, pos, Game1.PIXEL_SCALE, Game1.PIXEL_SCALE, true, "PlayerHang");
        anims[(int)animStates.Dying] = new Animation(imgs[(int)animStates.Dying], 23, 1, 23, 0, Animation.NO_IDLE, 1, 23 * FRAME_DURATION, pos, Game1.PIXEL_SCALE, Game1.PIXEL_SCALE, false, "PlayerDeath");
    }

    private void SetState(animStates newState, bool activate = true)
    {
        if (state != newState)
        {
            state = newState;
            anims[(int)state].Activate(activate);
        }
    }

    public void ResetPlayer()
    {
        // Reset default states for safety
        IsDead = false;
        isGrounded = false;
        onGround = false;
        ceilingAbove = false;
        isLatching = false;
        isClimbing = false;
        onZipline = false;

        inWater = false;
        prevInWater = false;
        isSliding = false;
        isPrevSliding = false;

        // Set oxygen back to max amount
        Oxygen = MAX_OXYGEN;

        // Reset death animation
        anims[(int)animStates.Dying].Activate(true);
    }

    private void LoadWin()
    {
        // Play win sound and pause the game
        SoundManager.PlayWin();
        Game1.paused = true;

        // Open win screen
        new WinScreen().AddToRoot();
    }

    private void StartPlayerDeath()
    {
        if (!IsDead)
        {
            IsDead = true;
            SoundManager.PlayDeath();
        }
    }

    public void Update(GameTime gameTime, KeyboardState kb, KeyboardState prevKb, Map map)
    {
        // Move player, check collisions, update player attributes
        prevPos = pos;
        Move(gameTime, kb, prevKb);
        UpdateRec();

        CheckCollisions(gameTime, map, kb, prevKb);
        UpdateRec();
        UpdateOxygen(gameTime, map);

        // Update button indicator
        btnIndic.Update(this, Camera);

        // Update animations and camera
        UpdateAnims(gameTime);
        Camera.Update(gameTime, rec.Center.ToVector2(), onZipline);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        anims[(int)state].Draw(spriteBatch, Color.White, direction);
    }

    public void CenterPos(Vector2 newPos)
    {
        pos = newPos - new Vector2(rec.Width, rec.Height) / 2;
    }

    private void MoveChecks(GameTime gameTime, KeyboardState kb, KeyboardState prevKb)
    {
        // If player is hanging on a zipline, move on zipline
        if (onZipline)
        {
            curZipline.MovePlayer(this, ZIPLINE_SPEED);
            return;
        }

        // If player is latched, their only action is to jump off
        if (isLatching)
        {
            // If player jumps, launch off the wall jump
            if ((kb.IsKeyDown(Keys.W) && !prevKb.IsKeyDown(Keys.W)) || (kb.IsKeyDown(Keys.Up) && !prevKb.IsKeyDown(Keys.Up)) || (kb.IsKeyDown(Keys.Space) && !prevKb.IsKeyDown(Keys.Space)))
            {
                vel.X = moveInput * WALL_JUMP_SPEED;
                vel.Y = JUMP_SPEED;

                isLatching = false;
                SoundManager.PlayWalljumpOff();
            }

            return;
        }

        // Move the player left or right based on player input
        moveInput = 0;

        if (kb.IsKeyDown(Keys.A) || kb.IsKeyDown(Keys.Left))
        {
            moveInput = -1;
        }
        else if (kb.IsKeyDown(Keys.D) || kb.IsKeyDown(Keys.Right))
        {
            moveInput = 1;
        }

        // Change player movement depending on swimming or not
        if (inWater)
        {
            // Slow player down by water resistance
            vel.X = vel.X - Math.Sign(vel.X) * Math.Min(Math.Abs(vel.X), WATER_RESISTANCE * (float)gameTime.ElapsedGameTime.TotalSeconds);
            vel.Y = vel.Y - Math.Sign(vel.Y) * Math.Min(Math.Abs(vel.Y), WATER_RESISTANCE * (float)gameTime.ElapsedGameTime.TotalSeconds);

            // Swim up or swim down instead of jump/dive
            if (kb.IsKeyDown(Keys.W) || kb.IsKeyDown(Keys.Up) || kb.IsKeyDown(Keys.Space))
            {
                vel.Y -= SWIM_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (kb.IsKeyDown(Keys.S) || kb.IsKeyDown(Keys.Down))
            {
                vel.Y += SWIM_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            // Swim horizontally
            vel.X += moveInput * SWIM_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Clamp swimming speeds
            vel.X = Math.Clamp(vel.X, -RUN_SPEED, RUN_SPEED);
            vel.Y = Math.Clamp(vel.Y, -RUN_SPEED, RUN_SPEED);

            return;
        }

        // Update x velocity, but don't reset when in air or swimming
        if (moveInput != 0)
        {
            // If in air, simulate less ground friction
            if (isGrounded)
            {
                vel.X = moveInput * RUN_SPEED;
            }
            else
            {
                // If moving same direction, choose largest movement, otherwise changing direction means slowly transitioning
                if (Math.Sign(vel.X) == Math.Sign(moveInput) || Math.Abs(vel.X) < DEFAULT_THRESHOLD)
                {
                    vel.X = moveInput * Math.Max(Math.Abs(vel.X), RUN_SPEED);
                }
                else
                {
                    vel.X += moveInput * RUN_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
        }

        // Update the player's speed by player's fall speed when airborne
        if (!isGrounded && vel.Y < MAX_FALL_SPEED) 
        {
            vel.Y = Math.Min(MAX_FALL_SPEED, vel.Y + GRAVITY * (float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        // Slow player down by air resistance
        vel.X = vel.X - Math.Sign(vel.X) * Math.Min(Math.Abs(vel.X), AIR_RESISTANCE * (float)gameTime.ElapsedGameTime.TotalSeconds);

        // If the player presses a jump key, and is on the grounded, update their speed to move up and play jump animation (if no ceiling above)
        if ((isGrounded || prevInWater || isClimbing) && !isSliding && !ceilingAbove
            && (kb.IsKeyDown(Keys.W) || kb.IsKeyDown(Keys.Up) || kb.IsKeyDown(Keys.Space)))
        {
            // Update the player's velocity, making them jump
            vel.Y = isGrounded || prevInWater ? JUMP_SPEED : -CLIMB_SPEED;

            // Since the player jumped, they are no longer grounded
            isGrounded = false;
            coyoteTime = -1;

            // Play jump animation (if not climbing)
            if (!isClimbing) SetState(animStates.Jumping);
        }

        // Set player as assumed to not be sliding
        isPrevSliding = isSliding;
        isSliding = false;

        // Slide or airdive if the player presses the button to, depending on if the player is on the ground or not
        if (onGround)
        {
            // If the player is holding slide key while on ground, he is sliding
            if (kb.IsKeyDown(Keys.S) || kb.IsKeyDown(Keys.Down))
            {
                isSliding = true;
            }
        }
        else if (!isGrounded && ((kb.IsKeyDown(Keys.S) && !prevKb.IsKeyDown(Keys.S)) || (kb.IsKeyDown(Keys.Down) && !prevKb.IsKeyDown(Keys.Down))))
        {
            // Update the player's velocity, making them air dive
            vel.Y = DIVE_SPEED;
        }

        // If the player is not diving, clamp their speed on ladders
        if (isClimbing && vel.Y > DEFAULT_THRESHOLD && vel.Y < DIVE_SPEED - DEFAULT_THRESHOLD)
        {
            vel.Y = Math.Min(vel.Y, CLIMB_SPEED);
        }
    }

    private void Move(GameTime gameTime, KeyboardState kb, KeyboardState prevKb)
    {
        // If the player is dead, do not let them move
        if (IsDead) return;

        // Perform movement checks
        MoveChecks(gameTime, kb, prevKb);

        // Clamp the speeds to make sure the player does not exceed their max speed
        vel.X = MathHelper.Clamp(vel.X, -MAX_SPEED, MAX_SPEED);
        vel.Y = MathHelper.Clamp(vel.Y, -MAX_SPEED, MAX_SPEED);

        // Update the player's position with velocity
        pos.X += vel.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
        pos.Y += vel.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;

        // If the player is in water, they aren't sliding anymore
        if (!prevInWater && inWater && isSliding)
        {
            isPrevSliding = false;
            isSliding = false;
        }
    }

    private void UpdateRec()
    {
        // Make the height halved when swimming or sliding
        int newHeight = (int)(TRUE_HEIGHT * (inWater || isSliding ? HEIGHT_SHRINK_FACTOR : 1));

        // If height changed, ensure player always grounded
        if (newHeight != rec.Height) pos.Y += rec.Height - newHeight;

        // Update height and position
        rec.Height = newHeight;
        rec.X = (int)pos.X;
        rec.Y = (int)pos.Y;
    }

    private void LoadCollisionRecs()
    {
        // Set up player collision direction / body part collision rectangles
        top = new Rectangle((int)(rec.X + 0.2 * rec.Width), rec.Y, (int)(0.6 * rec.Width), (int)(0.25 * rec.Height));
        bottom = new Rectangle((int)(rec.X + 0.15 * rec.Width), (int)(rec.Y + 0.75 * rec.Height), (int)(0.7 * rec.Width), (int)(0.25 * rec.Height));
        left = new Rectangle(rec.X, (int)(rec.Y + 0.05 * rec.Height), (int)(0.5 * rec.Width), (int)(0.9 * rec.Height));
        right = new Rectangle((int)(rec.X + 0.5 * rec.Width), (int)(rec.Y + 0.05 * rec.Height), (int)(0.5 * rec.Width), (int)(0.9 * rec.Height));
    }

    private void CheckCollisions(GameTime gameTime, Map map, KeyboardState kb, KeyboardState prevKb)
    {
        // If the player falls out of the world, they die
        if (pos.Y > (map.SizeY + FALL_DEATH_THRESHOLD) * Game1.TILE_SIZE * Game1.PIXEL_SCALE) StartPlayerDeath();

        // Load rectangles for checking collisions
        LoadCollisionRecs();

        // Get the current tile of the map the player is on (slightly lower point for water tile check), and create variables for storing tile type and recs (Math.Floor to always round down to negative infinity)
        (int tileX, int tileY) = Game1.CalcTile(rec.Center.ToVector2());
        Point waterTile = Game1.CalcTile(new Vector2(rec.Center.X, rec.Center.Y + rec.Height * WATER_LOWER_CENTER_AMOUNT));

        int tileType;
        Rectangle tileRec;

        // Reset player state checks
        isGrounded = onGround = false;
        ceilingAbove = false;
        isClimbing = false;

        prevInWater = inWater;
        inWater = false;

        // If the player is latching onto a wall, they aren't grounded
        if (isLatching)
        {
            isGrounded = onGround = false;
            vel = Vector2.Zero;
        }

        // Perform tile collision checks with player (for the tile the player is at) (if tile exists)
        if (0 <= tileX && tileX < map.SizeX && 0 <= tileY && tileY < map.SizeY)
        {
            tileType = map.Tiles[tileX, tileY].Type;

            // Check each special tile
            if (tileType == (int)Tile.Func.End)
            {
                // If the player reaches the end, then win only if they got all the buttons
                if (Buttons.IsEmpty())
                {
                    LoadWin();
                    return;
                }
            }
            else if (Tile.GetType(tileType) == (int)Tile.Func.Button)
            {
                // Check if button is found
                if (tileX == NextButton.X && tileY == NextButton.Y)
                {
                    // Remove button and store next one
                    Buttons.Delete(NextButton);
                    NextButton = Buttons.GetLeftmost();

                    // Play button pickup sound and give screen glow for player
                    SoundManager.PlayButton();
                    btnGlow = 1;
                }
            }
            else if (Tile.GetType(tileType) == Tile.ZIPLINE)
            {
                // Player either enters or exits zipline
                if (Zipline.IsStart(tileType))
                {
                    // Only put player on this zipline if not already on a zipline
                    if (!onZipline)
                    {
                        onZipline = true;
                        curZipline = map.Ziplines[Zipline.GetId(tileType)];
                        SoundManager.PlayZiplineStart();

                        // Center player at zipline
                        CenterPos(map.Tiles[tileX, tileY].Rec.Center.ToVector2());
                    }
                }
                else if (onZipline && curZipline.End.Type == tileType)
                {
                    onZipline = false;
                    SoundManager.PlayZiplineEnd();
                }
            }
            else if (waterTile.X >= 0 && waterTile.X < map.SizeX && waterTile.Y >= 0 && waterTile.Y < map.SizeX
                && (Tile.GetType(map.Tiles[waterTile.X, waterTile.Y].Type) == (int)Tile.Func.Water || map.FloodTiles[waterTile.X, waterTile.Y] == Tile.FLOODED))
            {
                // Player is in water
                inWater = true;

                // Play underwater sfx
                SoundManager.PlayUnderwater();
            }
            else if (tileType == (int)Tile.Decor.Ladder)
            {
                // Player is climbing
                isClimbing = true;
            }
        }

        // If player is not in water stop underwater sfx
        if (!inWater) SoundManager.StopUnderwater();

        // Check for platform collisions within a certain radius of the player (if not on zipline)
        if (!onZipline)
        {
            for (int x = Math.Max(0, tileX - TILE_CHECK_SIZE); x <= Math.Min(map.SizeX - 1, tileX + TILE_CHECK_SIZE); x++)
            {
                for (int y = Math.Max(0, tileY - TILE_CHECK_SIZE); y <= Math.Min(map.SizeY - 1, tileY + TILE_CHECK_SIZE); y++)
                {
                    if (map.Tiles[x, y] == null) continue;
                    tileType = map.Tiles[x, y].Type;
                    tileRec = map.Tiles[x, y].Rec;

                    // Check if any collision occurred with a collidable tile
                    if (Tile.CanCollide(tileType) && rec.Intersects(tileRec))
                    {
                        // Check if player collides up or down
                        if (tileRec.Intersects(top))
                        {
                            // Update that there is ceiling above
                            ceilingAbove = true;

                            // If the player just exited sliding, keep them in sliding as there is a platform preventing them from standing up
                            if (isPrevSliding && !isSliding)
                            {
                                // Put player back in sliding
                                isSliding = true;
                                isPrevSliding = true;
                                
                                // Update recs
                                UpdateRec();
                                LoadCollisionRecs();
                            }
                            else
                            {
                                // Set the player right below the platform, let upwards speed be blocked by the ceiling, letting gravity take over
                                pos.Y = tileRec.Bottom + 1;
                                vel.Y = 0;
                            }
                        }
                        else if (tileRec.Intersects(bottom))
                        {
                            // Set the player right above the platform, let downwards speed be blocked by the floor
                            pos.Y = tileRec.Top - rec.Height + 1;
                            vel.Y = 0;

                            // Update grounded status
                            isGrounded = onGround = true;
                        }

                        // Check if player collides left or right
                        if (tileRec.Intersects(left))
                        {
                            // If sliding and collision isn't very far, don't collide (avoid snagging while sliding)
                            if (onGround && (isSliding || isPrevSliding) && Math.Min(left.Right, tileRec.Right) - Math.Max(left.Left, tileRec.Left) <= X_OVERLAP_THRESHOLD) continue;

                            // Set the player right to the right of the platform, let leftwards speed be blocked by the floor
                            pos.X = tileRec.Right + 1;
                            vel.X = 0;

                            // If the player hits a wall jump, latch onto it (if not swimming)
                            if (!inWater && tileType == (int)Tile.Func.WallJump && tileRec.Contains(new Vector2(rec.Left, rec.Center.Y)))
                            {
                                isLatching = true;
                                moveInput = 1;
                                direction = SpriteEffects.None;
                                SoundManager.PlayWalljumpOn();
                            }
                        }
                        else if (tileRec.Intersects(right))
                        {
                            // If sliding and collision isn't very far, don't collide (avoid snagging while sliding)
                            if (onGround && (isSliding || isPrevSliding) && Math.Min(right.Right, tileRec.Right) - Math.Max(right.Left, tileRec.Left) <= X_OVERLAP_THRESHOLD) continue;

                            // Set the player right to the left of the platform, let rightwards speed be blocked by the floor
                            pos.X = tileRec.Left - rec.Width;
                            vel.X = 0;

                            // If the player hits a wall jump, latch onto it (if not swimming)
                            if (!inWater && tileType == (int)Tile.Func.WallJump && tileRec.Contains(new Vector2(rec.Right, rec.Center.Y)))
                            {
                                isLatching = true;
                                moveInput = -1;
                                direction = SpriteEffects.FlipHorizontally;
                                SoundManager.PlayWalljumpOn();
                            }
                        }
                    }
                }
            }
        }

        // Update climb down timer
        climbDownTimer.Update(gameTime.ElapsedGameTime.Milliseconds);

        // Perform special collision checks with the ladder (only if player is not jumping/climbing/swimming) (and the game thinks they aren't grounded)
        if (!onZipline && !isGrounded && !inWater && 0 <= tileX && tileX < map.SizeX)
        {
            for (int y = Math.Max(0, tileY); y <= Math.Min(map.SizeY - 1, tileY + 1); y++)
            {
                // Store tile to check
                Tile tile = map.Tiles[tileX, y];

                // Only check collisions if ladder and colliding with feet while not going up
                if (tile.Type == (int)Tile.Decor.Ladder && tile.Rec.Intersects(bottom) && vel.Y > -DEFAULT_THRESHOLD)
                {
                    // Check if player is trying to climb down
                    if ((kb.IsKeyDown(Keys.S) && !prevKb.IsKeyDown(Keys.S)) || (kb.IsKeyDown(Keys.Down) && !prevKb.IsKeyDown(Keys.Down)))
                    {
                        // Activate the timer to buffer
                        climbDownTimer.ResetTimer(true);
                    }

                    // Check if player is climbing down, make them go down
                    if (climbDownTimer.IsActive() && (kb.IsKeyDown(Keys.S) || kb.IsKeyDown(Keys.Down)))
                    {
                        vel.Y = CLIMB_SPEED;
                    }

                    // If player is not climbing down, collide player with the ladder step 
                    if (!climbDownTimer.IsActive() && !kb.IsKeyDown(Keys.S) && !kb.IsKeyDown(Keys.Down))
                    {
                        // Set the player right above the platform, let downwards speed be blocked by the floor
                        pos.Y = tile.Rec.Top - rec.Height + 1;
                        vel.Y = 0;

                        // Update grounded status
                        isGrounded = true;
                    }
                }
            }
        }

        // If just exited water, check if there's a platform above, if so force crawl
        if (prevInWater && !inWater && !isSliding)
        {
            // Store variables to check standing
            int fullHeight = (int)(TRUE_HEIGHT * 1f);
            int shrunkHeight = (int)(TRUE_HEIGHT * HEIGHT_SHRINK_FACTOR);
            Rectangle standTest = new Rectangle(rec.X, (int)pos.Y - (fullHeight - shrunkHeight), rec.Width, fullHeight);

            // Search through all tiles
            for (int x = Math.Max(0, tileX - TILE_CHECK_SIZE); x <= Math.Min(map.SizeX - 1, tileX + TILE_CHECK_SIZE); x++)
            {
                for (int y = Math.Max(0, tileY - TILE_CHECK_SIZE); y <= Math.Min(map.SizeY - 1, tileY + TILE_CHECK_SIZE); y++)
                {
                    // If tile does not exist, don't do any logic checks for safety, otherwise store type
                    if (map.Tiles[x, y] == null) continue;
                    tileType = map.Tiles[x, y].Type;

                    // If any valid head hitting tiles collide with standing rec, force slide
                    if (Tile.CanCollide(tileType) && standTest.Intersects(map.Tiles[x, y].Rec))
                    {
                        // No room to stand, force slide
                        isSliding = true;
                        break;
                    }
                }

                // Quick exit if already detected player should be forced to slide
                if (isSliding) break;
            }
        }

        // Let player be grounded for a short time frame even after not detecting ground collision
        if (isGrounded)
        {
            coyoteTime = COYOTE_MAX;
        }
        else if (coyoteTime > 0)
        {
            isGrounded = true;
            coyoteTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }

    private void UpdateOxygen(GameTime gameTime, Map map)
    {
        // If in water, lose oxygen, otherwise gain oxygen
        if (inWater)
        {
            Oxygen -= map.DrownSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        else
        {
            Oxygen = Math.Min(MAX_OXYGEN, Oxygen + INHALE_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        // If the player runs out of oxygen, they die
        if (Oxygen < 0) StartPlayerDeath();
    }

    private void UpdateAnimsChecks()
    {
        // Perform game over logic
        if (IsDead)
        {
            // Stop underwater
            SoundManager.StopUnderwater();

            // Play death animation if dead, but if animation done go to game over screen
            if (anims[(int)animStates.Dying].IsFinished())
            {
                // Pause the game
                Game1.paused = true;

                // Open gameover screen
                new DeathScreen().AddToRoot();
            }
            else SetState(animStates.Dying);
            return;
        }

        // Flip sprite based on player movement direction if the player is not latching
        if (vel.X > 0) direction = Animation.FLIP_NONE;
        else if (vel.X < 0) direction = Animation.FLIP_HORIZONTAL;

        // Play latching animation when needed
        if (isLatching)
        {
            SetState(animStates.Latching);
            return;
        }

        // If the player is on a zipline, play zipline hanging animation
        if (onZipline)
        {
            // Play zipline sfx (if not playing)
            SoundManager.PlayZiplineDuring();

            SetState(animStates.Hanging);
            return;
        }

        // If the player is in water, they are in the swimming animation
        if (inWater)
        {
            SetState(animStates.Swimming);
            return;
        }
        
        // If airborne, check latching, on ladder, or if fall or jump animation should be playing (or default to idle)
        if (!isGrounded)
        {
            // If falling too fast, no climb animation, if not latch check jump/fall/idle animations
            if (isClimbing && vel.Y <= CLIMB_SPEED + DEFAULT_THRESHOLD)
            {
                SetState(animStates.Climbing, false);
            }
            else if (!isLatching)
            {
                if (vel.Y < 0) SetState(animStates.Jumping);
                else if (vel.Y >= FALL_THRESHOLD) SetState(animStates.Falling);
                else if (state != animStates.Jumping && state != animStates.Running) SetState(animStates.Idle);
            }
            return;
        }
        
        // If sliding, play slide animation
        if (isSliding)
        {
            SetState(animStates.Sliding);
            return;
        }
        
        // Play landing animation as we just landed only if no input
        if ((state == animStates.Falling || state == animStates.Jumping) && moveInput == 0)
        {
            SetState(animStates.Landing);
            return;
        }
        
        // When the landing animation finishes, go to Idle
        if (state == animStates.Landing)
        {
            if (!anims[(int)animStates.Landing].IsAnimating()) SetState(animStates.Idle);
            return;
        }
        
        // Player is trying to move
        if (moveInput != 0)
        {
            SetState(animStates.Running);
            return;
        }
        
        // Otherwise idle
        SetState(animStates.Idle);
    }

    private void UpdateAnims(GameTime gameTime)
    {
        // Check which animation should be done
        UpdateAnimsChecks();

        // Update the position of the animation
        anims[(int)state].TranslateTo(pos.X - (ANIM_SIZE * Game1.PIXEL_SCALE - rec.Width) / 2, pos.Y - (ANIM_SIZE * Game1.PIXEL_SCALE - rec.Height) / 2);

        // Update button screen glow amount and update button vignette
        btnGlow = Math.Max(0, btnGlow - Game1.ExpSmoothing(gameTime, btnGlowSpeed));
        Game1.playScreen.BtnVignette.Alpha2 = (int)(255 * btnGlow);

        // Update animation, unless the frame should be frozen
        if ((inWater && Math.Abs(vel.X) + Math.Abs(vel.Y) >= DEFAULT_THRESHOLD)
            || (!inWater && (!isSliding || isClimbing || moveInput != 0))
            || IsDead)
        {
            anims[(int)state].Update(gameTime);
        }
    }
}