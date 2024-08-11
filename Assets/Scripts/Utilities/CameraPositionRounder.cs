/**************************************************
 *  CameraPositionRounder.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.HuntTheMuglump.Scripts.Utilities
{
    using Cinemachine;

    /// <summary>
    /// Extends the Cinemachine class to smooth the camera positioning
    /// </summary>
    /// <seealso cref="Cinemachine.CinemachineExtension" />
    public class CameraPositionRounder : CinemachineExtension
    {
        /// <summary>
        /// Posts the pipeline stage callback.
        /// </summary>
        /// <param name="vcam">The virtual camera.</param>
        /// <param name="stage">The stage.</param>
        /// <param name="state">The state.</param>
        /// <param name="deltaTime">The delta time.</param>
        protected override void PostPipelineStageCallback(
            CinemachineVirtualCameraBase vcam,
            CinemachineCore.Stage stage,
            ref CameraState state,
            float deltaTime)
        {
            if (stage == CinemachineCore.Stage.Body)
            {
                //var exactPosition = state.FinalPosition;
                //var roundedPosition = exactPosition.Round(Constants.PixelsPerUnit);

                //state.PositionCorrection += roundedPosition - exactPosition;
            }
        }
    }
}
